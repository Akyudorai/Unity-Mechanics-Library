using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TPSC_RPG1 : TPS_Controller {

    // #########################################################
    // # --------------- CONTROLLER COMPONENTS --------------- #
    // #########################################################
    
    [Header("Controller Components")]
    [SerializeField] UIRaycaster uiRaycaster;
    
    CharacterController controller;

    TPS_Entity entity;
    public TPS_Entity GetEntity() { return entity; }
    TPS_Hotbar hotbar;

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################

    [Header("Motion Settings")] // m_
    [SerializeField] [Range(0.1f, 3.0f)] float m_Speed = 0.15f;    
    //[SerializeField] [Range(0.1f, 0.5f)] float m_JumpForce = 0.25f;
    private float m_xForce, m_yForce, m_zForce;
    private bool isFalling;
    //private bool m_Jump;
    private Vector2 ForceVector;
    public float speedMultiplier = 1.0f;
    
    [Header("Class System")]
    [SerializeField] RPG3_ClassList selectClass;
    private RPG3_Class currentClass;
    public RPG3_Class GetClass() { return currentClass; }

    public RPG3_Ability channelledAbility;
    public float channelTimer;
    public bool isChanneling;
    [SerializeField] Image channelingBar;
    [SerializeField] Text channelingText;
    public TPS_Controller channelTarget;

    [Header("Targeting System")]
    [SerializeField] TPS_Controller targetController;
    [SerializeField] TPS_Entity targetEntity;
    public TPS_Controller GetCurrentTarget() { return targetController; }
    public TPS_Entity GetCurrentTargetEntity() { return targetEntity; }
    
    public GameObject activeTargetIndicator;    
    public RPG3_Ability abilityBeingTargeted;
    private Vector3 abilityTargetPoint;

    // Targeting System GUI
    [SerializeField] GameObject targetInfoDisplay;
    [SerializeField] Image targetHealthBar;
    [SerializeField] Image targetResourceBar;
    [SerializeField] Text targetHealthText;
    [SerializeField] Text targetResourceText;

    [Header("Player GUI")]
    [SerializeField] Canvas guiCanvas;

    // Player Management
    [SerializeField] Image healthBar;
    public Image resourceBar;
    [SerializeField] Text healthText;
    [SerializeField] Text resourceText;

    // Ability Management Window
    [SerializeField] GameObject abilityManager;
    
    // Character Info Window
    [SerializeField] GameObject characterInfo;

    
   

    // #########################################################
    // -------------------- INITIALIZATION ------------------- #
    // #########################################################

    protected override void Awake()
    {
        base.Awake();

        entity = GetComponent<TPS_Entity>();
        controller = GetComponent<CharacterController>();
        hotbar = GetComponent<TPS_Hotbar>();

        // Values
        isFalling = true;

        if (owner != null)
        {
            // Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        else
        {
            guiCanvas.enabled = false;
        }

        switch (selectClass)
        {
            default:
            case RPG3_ClassList.Warrior:
                currentClass = ScriptableObject.CreateInstance<RPG3_Warrior>();                
                break;
            
            case RPG3_ClassList.Rogue:
                currentClass = ScriptableObject.CreateInstance<RPG3_Rogue>();                
                break;

            case RPG3_ClassList.Mage:
                currentClass = ScriptableObject.CreateInstance<RPG3_Mage>();
                break;
            
        }

        for (int i = 0; i < currentClass.abilities.Count; i++)
        {
            abilityManager.GetComponent<RPG3_AbilityPanel>().Initialize(this, currentClass.abilities[i]);
        }

        switch (currentClass.resourceType)
        {
            case RPG3_ResourceType.Rage:
                resourceBar.color = Color.red;
                entity.maxHealth = currentClass.baseHealth + (currentClass.stamina * 8);
                entity.maxResource = currentClass.baseResource;
                entity.resource = 0;                
                break;

            case RPG3_ResourceType.Mana:
                resourceBar.color = Color.blue;
                entity.maxHealth = currentClass.baseHealth + (currentClass.stamina * 8);                
                entity.maxResource = currentClass.baseResource + (currentClass.intellect * 8);
                entity.resource = entity.maxResource;
                break;

            case RPG3_ResourceType.Energy:
                resourceBar.color = Color.yellow;
                entity.maxHealth = currentClass.baseHealth + (currentClass.stamina * 8);                
                entity.maxResource = currentClass.baseResource;
                entity.resource = entity.maxResource;
                break;

            default: break;
        }

        currentClass.entity = entity;
        characterInfo.GetComponent<RPG3_CharacterInfoManager>().controller = this;        
        //hotbar.UpdateIcons();
    }

    private void Start()
    {
        delay = GCD;

        if (targetEntity == null)
        {
            targetInfoDisplay.SetActive(false);
        }
               
    }

    // #########################################################
    // # ---------------------- UPDATE ----------------------- #
    // #########################################################

    public override void Tick()
    {
        base.Tick();
        
        if (owner != null)
        {
            if (!owner.gm.isPaused)
            {
                if (!disableControls)
                {
                    Motion();
                }
                
                
                // TARGETING SYSTEM
                if (activeTargetIndicator != null)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        // Get Collision Data
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        
                        activeTargetIndicator.transform.position = hitInfo.point + hitInfo.normal * 0.1f;
                    }
                    
                }
                
                // CHANNELLING
                if (channelledAbility != null)
                {                               
                    channelingBar.enabled = true;
                    channelingText.enabled = true;

                    if (channelTimer > 0)
                    {                        
                        channelTimer -= Time.deltaTime;
                        isChanneling = true;
                        
                        channelingText.text = channelledAbility.GetName() + " : " + Mathf.Round(channelTimer * 100) / 100;
                        channelingBar.fillAmount = (channelTimer / 2.5f);
                    }

                    if (channelTimer <= 0)
                    {
                        RPG3_AbilitySettings settings = new RPG3_AbilitySettings();
                        switch (channelledAbility.GetAbilityMode())
                        {
                            case RPG3_AbilityMode.Targeted:
                                settings = RPG3_AbilitySettings.Initialize(this, channelTarget);
                                break;

                            case RPG3_AbilityMode.Positional:
                                settings = RPG3_AbilitySettings.Initialize(this, abilityTargetPoint);
                                break;

                             case RPG3_AbilityMode.Self:
                                settings = RPG3_AbilitySettings.Initialize(this, channelTarget);
                                break;
                        }
                                               
                        channelledAbility.Effect(settings);
                        
                        channelledAbility = null;
                        channelTarget = null;
                        channelingBar.enabled = false;
                        channelingText.enabled = false;
                    }
                }                

                hotbar.HotkeyTracker();
                
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {                    
                    // If a UI element was pressed, break out.
                    if (uiRaycaster.Raycast().Count > 0)
                    {
                        return;
                    }

                    if (abilityBeingTargeted == null)
                    {
                        // Clear the active target if worldspace was clicked
                        if (targetController != null || targetEntity != null)
                        {
                            targetController = null;
                            targetEntity = null;
                        }
                    }
                    

                    // if the UI was not pressed, raycast to world position
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        // Get Collision Data
                        ///////////////////////////////////////////////////////////////////////////////////////////////

                        GameObject targetHit = hitInfo.collider.gameObject;
                        Debug.Log(targetHit);

                        // Activate an ability that's being targeted
                        if (abilityBeingTargeted != null)
                        {
                            if (activeTargetIndicator != null)
                            {
                                Destroy(activeTargetIndicator);
                                activeTargetIndicator = null;
                            }

                            RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(this, hitInfo.point);                            
                            abilityBeingTargeted.Activate(settings);

                            abilityTargetPoint = hitInfo.point;
                            abilityBeingTargeted.isTargeting = false;
                            abilityBeingTargeted = null;
                        }


                        // Selection Command
                        if (targetHit.GetComponentInParent<TPS_Controller>() != null)
                        {
                            targetController = targetHit.GetComponentInParent<TPS_Controller>();                           
                        }

                        if (targetHit.GetComponentInParent<TPS_Entity>() != null)
                        {
                            //Debug.Log(hitInfo.collider.gameObject.name + " selected");                    
                            
                            targetEntity = targetHit.GetComponentInParent<TPS_Entity>();

                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        }                     
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    // Activate an ability that's being targeted
                    if (abilityBeingTargeted != null)
                    {
                        if (!isChanneling)
                        {
                            if (activeTargetIndicator != null)
                            {
                                Destroy(activeTargetIndicator);
                                activeTargetIndicator = null;
                            }

                            abilityTargetPoint = Vector3.zero;
                            abilityBeingTargeted.isTargeting = false;
                            abilityBeingTargeted = null;
                        }                        
                    }
                }

                // Update Canvas
                if (guiCanvas.enabled)
                {
                    // Health
                    healthText.text = entity.GetCurrentHealth() + " / " + entity.GetMaxHealth();

                    float currentHealth = entity.GetCurrentHealth();
                    float maxHealth = entity.GetMaxHealth();
                    healthBar.fillAmount = currentHealth / maxHealth;
                    
                    // Resource
                    resourceText.text = entity.GetCurrentResource() + " / " + entity.GetMaxResource();

                    float currentResource = entity.GetCurrentResource();
                    float maxResource = entity.GetMaxResource();
                    resourceBar.fillAmount = currentResource / maxResource;

                    // Target System
                    if (targetEntity != null)
                    {
                        if (!targetInfoDisplay.activeInHierarchy)
                            targetInfoDisplay.SetActive(true);

                        // Health
                        targetHealthText.text = targetEntity.GetCurrentHealth() + " / " + targetEntity.GetMaxHealth();

                        float currentTargetHealth = targetEntity.GetCurrentHealth();
                        float maxTargetHealth = targetEntity.GetMaxHealth();
                        targetHealthBar.fillAmount = currentTargetHealth / maxTargetHealth;

                        // Resource
                        targetResourceText.text = targetEntity.GetCurrentResource() + " / " + targetEntity.GetMaxResource();

                        float currentTargetResource = targetEntity.GetCurrentResource();
                        float maxTargetResource = targetEntity.GetMaxResource();
                        targetResourceBar.fillAmount = currentTargetResource / maxTargetResource;
                    }

                    else
                    {
                        if (targetInfoDisplay.activeInHierarchy)
                            targetInfoDisplay.SetActive(false);
                    }
                }       
            }           
        }
    }

    public void LateUpdate()
    {
        if (currentClass != null)
            currentClass.Tick();

        if (entity.GetCurrentHealth() <= 0)
        {
            entity.Destruct();
        }
    }

    // #########################################################
    // # --------------------- CONTROLS ---------------------- #
    // #########################################################

    // Player Movement Controls
    private void Motion()
    {
        if (isChanneling)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Debug.Log("Cancel Channeling");
                isChanneling = false;
                channelledAbility = null;

                channelTarget = null;
                channelingText.enabled = false;
                channelingBar.enabled = false;
            }
            
        }


        // X/Y Motion
        m_xForce = Input.GetAxis("Horizontal") * m_Speed * speedMultiplier;
        m_yForce = Input.GetAxis("Vertical") * m_Speed * speedMultiplier;

        // Z Motion
        //if (Input.GetKeyDown(KeyCode.Space) && m_Jump)
        //{
        //    m_zForce += m_JumpForce;
        //    m_Jump = false;

        //}

        if (!controller.isGrounded && isFalling)
        {
            m_zForce += (-0.15f) * Time.deltaTime;
        }
        else
        {
            m_zForce = 0;
            //m_Jump = true;
        }

        // Calculate Motion
        Vector3 motion = new Vector3(m_xForce, m_zForce, m_yForce);
        motion = transform.rotation * motion;
        controller.Move(motion);

    }

    protected override void Camera()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            base.Camera();
        }
    }

    // Player Interaction Controls
    private void Interact()
    {

        // Interact
        if (Input.GetKeyDown(KeyCode.E) && delay == 0)
        {
            // Set GCD for interact action
            delay = GCD;

            
        }

    }


    public override TPS_Controller Possess(TPS_Player player)
    {
        guiCanvas.enabled = true;

        // Cursor -- Confined lock mode doesn't work in editor, but does in standalone game.
        // For testing purposes, we'll use None as our lockmode in the editor.
        if (Application.isEditor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        

        return base.Possess(player);
    }

    public override void Eject()
    {
        base.Eject();

        guiCanvas.enabled = false;        
    }  
    
    //////////////////////////////////////////
    ///

    public void EnableAbilityPanel()
    {
        abilityManager.SetActive(!abilityManager.activeInHierarchy);
    }

    public void EnableCharacterInfoPannel()
    {
        characterInfo.SetActive(!characterInfo.activeInHierarchy);
    }

    public IEnumerator SpeedBuff(float multplier, float duration)
    {
        Debug.Log("SpeedBuff");

        speedMultiplier = 1.2f;

        yield return new WaitForSeconds(duration);

        speedMultiplier = 1.0f;
    }
}
