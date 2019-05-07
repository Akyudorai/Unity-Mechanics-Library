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
    
    [Header("Class System")]
    [SerializeField] RPG3_Class currentClass;
    public RPG3_Class GetClass() { return currentClass; }

    [Header("Targeting System")]
    [SerializeField] TPS_Controller targetController;
    [SerializeField] TPS_Entity targetEntity;
    public TPS_Controller GetCurrentTarget() { return targetController; }
    public TPS_Entity GetCurrentTargetEntity() { return targetEntity; }

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
    [SerializeField] Image resourceBar;
    [SerializeField] Text healthText;
    [SerializeField] Text resourceText;

    
   

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

    }

    private void Start()
    {
        delay = GCD;

        if (targetEntity == null)
        {
            targetInfoDisplay.SetActive(false);
        }

        currentClass = ScriptableObject.CreateInstance<RPG3_Warrior>();       
        hotbar.abilities[0] = currentClass.GetAbility("Rend");
        hotbar.abilities[1] = currentClass.GetAbility("Charge");

        hotbar.UpdateIcons();
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
                
                hotbar.HotkeyTracker();
                
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // If a UI element was pressed, break out.
                    if (uiRaycaster.Raycast().Count > 0)
                    {
                        return;
                    }

                    // Clear the active target if worldspace was clicked
                    if (targetController != null || targetEntity != null)
                    {
                        targetController = null;
                        targetEntity = null;
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

                // Update Hotbar
                for (int i = 0; i < hotbar.abilities.Length; i++)
                {
                    hotbar.icons[i].fillAmount = (1 - delay / GCD); 
                }          
            }           
        }
    }

    public void LateUpdate()
    {
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

        // X/Y Motion
        m_xForce = Input.GetAxis("Horizontal") * m_Speed;
        m_yForce = Input.GetAxis("Vertical") * m_Speed;

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
}
