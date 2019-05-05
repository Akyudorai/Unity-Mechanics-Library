using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_PlayerController : MonoBehaviour {

    // Game Manager Reference
    [SerializeField] GameManager gm;

    // #########################################################
    // # ---------------- CHARACTER VARIABLES ---------------- #
    // #########################################################

    [Header("Camera Settings")] // c_
    Camera cam;
    [SerializeField] float c_Sensitivity = 2.5f;
    [SerializeField] float c_ClampUp, c_ClampDown = 20.0f;
    private float c_hRot, c_vRot;

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
    [SerializeField] Image targetReticule;

    [SerializeField] Weapon equipped;

    // #########################################################
    // # ---------------------- UPDATE ----------------------- #
    // #########################################################

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();

        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Values
        isFalling = true;
    }

    private void Start()
    {
        if (equipped != null)
        {
            equipped.transform.localPosition = equipped.GetHandPosition();
        }

        delay = GCD;
    }

    private void Update()
    {
        if (!gm.isPaused)
        {
            Camera();
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

        if (equipped != null && gm.isPaused)
        {
            equipped.gunCanvas.enabled = false;
        }
        
    }

    // #########################################################
    // # --------------------- CONTROLS ---------------------- #
    // #########################################################

    // Player Camera Controls
    private void Camera()
    {

        // Horizontal Rotation
        float rotation_horizontal = Input.GetAxis("Mouse X") * c_Sensitivity;
        c_hRot += rotation_horizontal;

        gameObject.transform.Rotate(0, rotation_horizontal, 0);


        // Vertical Rotation & Clamping
        c_vRot -= Input.GetAxis("Mouse Y") * c_Sensitivity;
        c_vRot = Mathf.Clamp(c_vRot, -c_ClampUp, c_ClampDown);
        cam.transform.localRotation = Quaternion.Euler(c_vRot, 0, 0);

    }

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
}
