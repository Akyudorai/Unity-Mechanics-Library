using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TPSC_Shooter : TPS_Controller {

    // #########################################################
    // # --------------- CONTROLLER COMPONENTS --------------- #
    // #########################################################

    TPS_Entity entity;

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################

    [Header("Motion Settings")] // m_
    CharacterController controller;
    [SerializeField] [Range(0.1f, 3.0f)] float m_Speed = 0.15f;
    //[SerializeField] [Range(0.1f, 0.5f)] float m_JumpForce = 0.25f;
    private float m_xForce, m_yForce, m_zForce;
    private bool isFalling;
    //private bool m_Jump;
    private Vector2 ForceVector;

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
                Motion();


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
                }

                // GCD Timer
                if (delay > 0) delay -= Time.deltaTime;
                if (delay < 0) delay = 0;
            }

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

        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        return base.Possess(player);
    }

    public override void Eject()
    {
        base.Eject();

        guiCanvas.enabled = false;
    }
}
