using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : MonoBehaviour {

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################

    // Active Owner
    [SerializeField] protected FPS_Player owner;

    [Header("Camera Settings")] // c_
    [SerializeField] public Camera cam;
    [SerializeField] protected float c_Sensitivity = 2.5f;
    [SerializeField] protected float c_ClampUp, c_ClampDown = 20.0f;
    protected float c_hRot, c_vRot;

    // #########################################################
    // -------------------- INITIALIZATION ------------------- #
    // #########################################################

    protected virtual void Awake()
    {
        // Disable components if not possessed
        if (owner == null)
        {
            Eject();
        }
    }

    // #########################################################
    // # ---------------------- UPDATE ----------------------- #
    // #########################################################

    public virtual void Tick()
    {
        if (owner == null) return;
        
        Camera();
        
    }

    // #########################################################
    // # --------------------- CONTROLS ---------------------- #
    // #########################################################

    // Player Camera Controls
    protected virtual void Camera()
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

    // Take Control and Initialize all components of the controller
    public virtual FPS_Controller Possess(FPS_Player player)
    {
        Debug.Log(name + ": Possessing");

        owner = player;

        cam.enabled = true;
        cam.GetComponent<AudioListener>().enabled = true;
        cam.GetComponent<FlareLayer>().enabled = true;

        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        return this;
    }

    // Disable all the components of the controller and put it to sleep
    public virtual void Eject()
    {
        Debug.Log(name + " : Ejecting");

        owner = null;
        cam.enabled = false;
        cam.GetComponent<AudioListener>().enabled = false;
        cam.GetComponent<FlareLayer>().enabled = false;

    }
}
