using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class FPSC_Shooter1 : FPS_Controller {

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################
    
    [Header("Motion Settings")] // m_
    CharacterController controller;
    [SerializeField] [Range(0.1f, 3.0f)] float m_Speed = 0.15f;
    [SerializeField] [Range(0.1f, 0.5f)] float m_JumpForce = 0.25f;
    private float m_xForce, m_yForce, m_zForce;
    private bool isFalling, m_Jump;
    private Vector2 ForceVector;

    // Global Cooldown on Actions
    const float GCD = 0.25f;
    float delay;

    [Header("GUI")]
    [SerializeField] Canvas guiCanvas;
    [SerializeField] Image targetReticule;

    [SerializeField] Weapon equipped;

    // #########################################################
    // -------------------- INITIALIZATION ------------------- #
    // #########################################################

    protected override void Awake()
    {
        base.Awake();
        
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
        if (equipped != null)
        {
            equipped.transform.localPosition = equipped.GetHandPosition();
        }

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
                Interact();

                if (equipped != null)
                {
                    targetReticule.rectTransform.localScale = new Vector2(equipped.currentDeviationX * 10, equipped.currentDeviationY * 10);
                    //Debug.Log(equipped.currentDeviationX)


                    if (equipped.gunCanvas.enabled != true)
                        equipped.gunCanvas.enabled = true;


                    switch (equipped.GetTriggerType())
                    {
                        case FPS_TriggerType.SemiAuto:

                            // Fire Once Per Click
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                if (equipped != null)
                                {
                                    equipped.PrimaryFunction();
                                }
                            }

                            break;

                        case FPS_TriggerType.Automatic:

                            // Fire as long as you hold down the button
                            if (Input.GetKey(KeyCode.Mouse0))
                            {
                                if (equipped != null)
                                {
                                    equipped.PrimaryFunction();
                                }
                            }

                            break;

                        case FPS_TriggerType.Charged:

                            equipped.PrimaryFunction();

                            break;

                        default: break;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (equipped != null)
                        {
                            equipped.SecondaryFunction();
                            
                            if (equipped.isAiming)
                                equipped.transform.localPosition = equipped.GetAimedPosition();

                            else
                                equipped.transform.localPosition = equipped.GetHandPosition();
                        }

                    }


                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (equipped != null)
                        {
                            equipped.Release();
                            equipped = null;

                            targetReticule.rectTransform.localScale = new Vector2(0.1f, 0.1f);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (equipped != null)
                        {
                            if (equipped.GetCurrentClip() < equipped.GetClipSize())
                            {
                                equipped.Reload();
                            }
                        }
                    }

                }

                // GCD Timer
                if (delay > 0) delay -= Time.deltaTime;
                if (delay < 0) delay = 0;

            }

            if (equipped != null && owner.gm.isPaused)
            {
                equipped.gunCanvas.enabled = false;
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
        if (Input.GetKeyDown(KeyCode.Space) && m_Jump)
        {
            m_zForce += m_JumpForce;
            m_Jump = false;

        }

        if (!controller.isGrounded && isFalling)
        {
            m_zForce += (-0.5f) * Time.deltaTime;
        }
        else
        {
            m_zForce = 0;
            m_Jump = true;
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

            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if there's an interactable target in range.
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 5.0f))
            {
                // Declare Target;
                GameObject targetHit = hit.transform.gameObject;


                if (targetHit.GetComponent<I_Interactable>() != null)
                {
                    switch (targetHit.GetComponent<I_Interactable>().GetInteraction())
                    {
                        case FPS_InteractionType.Equippable:

                            if (equipped != null)
                            {
                                equipped.Release();
                                equipped = null;
                            }

                            if (targetHit.GetComponent<Weapon>() != null)
                            {
                                equipped = targetHit.GetComponent<Weapon>();
                                equipped.transform.SetParent(cam.transform);
                                equipped.Interact();
                            }

                            break;

                        case FPS_InteractionType.Button:

                            targetHit.GetComponent<FPS_Button>().Interact();

                            break;

                        default: break;
                    }

                    //Debug.Log(targetHit.name);
                }
            }
        }

    }


    public override FPS_Controller Possess(FPS_Player player)
    {
        guiCanvas.enabled = true;

        if (equipped != null)
            equipped.gunCanvas.enabled = true;

        return base.Possess(player);
    }

    public override void Eject()
    {
        base.Eject();

        guiCanvas.enabled = false;

        if (equipped != null)  
            equipped.gunCanvas.enabled = false;
    }
}
