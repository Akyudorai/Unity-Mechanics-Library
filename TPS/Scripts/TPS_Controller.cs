using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Controller : MonoBehaviour {

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################

    // Active Owner
    [SerializeField] protected TPS_Player owner;

    [Header("Camera Settings")] // c_
    [SerializeField] public Camera cam;
    [SerializeField] public GameObject cameraPivot;
    [SerializeField] protected float c_Sensitivity = 2.5f;
    [SerializeField] protected float c_ClampUp, c_ClampDown = 20.0f;
    protected float c_hRot, c_vRot;


    // Global Cooldown on Actions
    protected const float GCD = 0.5f;
    protected float delay;
    public float GetDelay() { return delay; }
    public void DelayGCD() { delay = GCD; }


    public bool disableControls;

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

        disableControls = false;
    }

    // #########################################################
    // # ---------------------- UPDATE ----------------------- #
    // #########################################################

    public virtual void Tick()
    {
        if (owner == null) return;

        Camera();
        
        // GCD Timer
        if (delay > 0) delay -= Time.deltaTime;
        if (delay < 0) delay = 0;

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
        cameraPivot.transform.localRotation = Quaternion.Euler(c_vRot, 0, 0);

    }

    // Take Control and Initialize all components of the controller
    public virtual TPS_Controller Possess(TPS_Player player)
    {
        Debug.Log(name + ": Possessing");

        owner = player;

        cam.enabled = true;
        cam.GetComponent<AudioListener>().enabled = true;
        cam.GetComponent<FlareLayer>().enabled = true;
        
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

    //public IEnumerator MoveTo(Vector3 targetPosition)
    //{
    //    yield return Time.deltaTime;

    //    float distance = Vector3.Distance(transform.position, targetPosition);
    //    float speed = 0.1f;

    //    float timeScale = speed / distance;

    //    transform.position = Vector3.Lerp(transform.position, targetPosition, timeScale);

    //    if (distance > 5.0f)
    //    {
    //        StartCoroutine(MoveTo(targetPosition));
    //    }

    //    else
    //    {
    //        disableControls = false;
    //    }

    //}

    public IEnumerator MoveToEntity(TPS_Entity targetEntity)
    {
        yield return Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetEntity.transform.position);
        float speed = 0.1f;

        float timeScale = speed / distance;

        transform.position = Vector3.Lerp(transform.position, targetEntity.transform.position, timeScale);

        if (distance > 5.0f)
        {
            StartCoroutine(MoveToEntity(targetEntity));
        }

        else
        {
            disableControls = false;
        }

    }

}
