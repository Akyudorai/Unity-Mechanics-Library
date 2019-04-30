using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum FPS_TriggerType
{
    SemiAuto,
    Automatic
}

public abstract class Weapon : MonoBehaviour, I_Interactable {

    // ########################################################
    // # ----------------- WEAPON COMPONENTS ---------------- #
    // ########################################################

    protected Camera cam;
    [SerializeField] protected Transform originPoint;
    protected LineRenderer line;
    protected Rigidbody rb;

    [Header("GUI")]
    public Canvas gunCanvas;
    [SerializeField] Text clipInfo;
    [SerializeField] Text ammoReserve;
    [SerializeField] Image reloadImg;


    [SerializeField] protected GameObject bullethole;

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    protected int currentClip; // Number of Shots in the clip
    protected int ammo; // Total Number of Shots in reserve
    protected bool isReloading; // State-based Boolean
    protected float reloadTimer;

    public float currentDeviationX;
    public float currentDeviationY;
    public bool isAiming;


    protected WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    protected float nextShot;

    // ########################################################
    // # ------------------ INITIALIZATION ------------------ #
    // ########################################################

    private void Awake()
    {
        if (cam == null)
            cam = GetComponentInParent<Camera>();

        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentClip = GetClipSize(); // 12 Shots
        ammo = GetMaxAmmo(); // 120 Reserve
        isReloading = false;
        reloadTimer = 0; // Current Reload Timer;

        currentDeviationX = GetShotDeviation().x;
        currentDeviationY = GetShotDeviation().y;
        isAiming = false;
    }

    // ########################################################
    // # ------------------- METHODOLOGY -------------------- #
    // ########################################################

    private void Update()
    {
        // Update Shot Timer
        if (nextShot > 0)
        {
            nextShot -= Time.deltaTime;
        }

        if (!isAiming)
        {
            if (currentDeviationX > GetShotDeviation().x)
            {
                if (currentDeviationX - (GetRecoil() / 15) >= GetShotDeviation().x)
                    currentDeviationX -= (GetRecoil() / 15);
                else if (currentDeviationX - (GetRecoil() / 15) < GetShotDeviation().x)
                    currentDeviationX = GetShotDeviation().x;
            }

            if (currentDeviationY > GetShotDeviation().y)
            {
                if (currentDeviationY - (GetRecoil() / 15) >= GetShotDeviation().y)
                    currentDeviationY -= (GetRecoil() / 15);
                else if (currentDeviationY - (GetRecoil() / 15) < GetShotDeviation().y)
                    currentDeviationY = GetShotDeviation().y;
            }
        }

        else if (isAiming)
        {
            if (currentDeviationX > GetAimedSpread())
            {
                if (currentDeviationX - (GetRecoil() / 15) >= GetAimedSpread())
                    currentDeviationX -= (GetRecoil() / 15);
                else if (currentDeviationX - (GetRecoil() / 15) < GetAimedSpread())
                    currentDeviationX = GetAimedSpread();
            }

            if (currentDeviationY > GetAimedSpread())
            {
                if (currentDeviationY - (GetRecoil() / 15) >= GetAimedSpread())
                    currentDeviationY -= (GetRecoil() / 15);
                else if (currentDeviationY - (GetRecoil() / 15) < GetAimedSpread())
                    currentDeviationY = GetAimedSpread();
            }
        }

        GUI();
    }

    public void Reload()
    {
        StartCoroutine(I_Reload());
    }

    private IEnumerator I_Reload()
    {
        isReloading = true;
        reloadTimer = GetReloadTime();

        yield return new WaitForSeconds(GetReloadTime());

        int refill = GetClipSize() - currentClip;

        if (ammo - refill >= 0)
        {
            currentClip = GetClipSize();
            ammo -= refill;
        }

        else if (ammo - refill < 0)
        {
            currentClip = ammo;
            ammo = 0;
        }

        isReloading = false;
    }

    // Update GUI Information
    private void GUI()
    {
        if (gunCanvas != null)
        {
            if (gunCanvas.enabled == true)
            {
                clipInfo.text = GetCurrentClip() + " / " + GetClipSize();
                ammoReserve.text = GetAmmo().ToString();

                if (IsReloading())
                {
                    if (!reloadImg.enabled) reloadImg.enabled = true;

                    // Adjust Fill Amount
                    reloadImg.fillAmount -= Time.deltaTime / GetReloadTimer();
                }

                else
                {
                    if (reloadImg.enabled) reloadImg.enabled = false;
                    if (reloadImg.fillAmount != 1.0f) reloadImg.fillAmount = 1.0f;
                }
            }

        }
    }

    // #########################################################
    // # ----------------- ABSTRACT METHODS ------------------ #
    // #########################################################

    public abstract void PrimaryFunction();
    public abstract void SecondaryFunction();

    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    /// I_Interactable
    public void Interact()
    {
        cam = GetComponentInParent<Camera>();
        transform.localPosition = GetHandPosition();
        transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

        gunCanvas.enabled = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        GetComponent<MeshCollider>().enabled = false;

        transform.localScale = new Vector3(0.65f, 0.8f, 0.65f);

    }

    /// I_Interactable
    public void Release()
    {
        // Rigidbody Settings
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(cam.transform.forward * 10.0f, ForceMode.Impulse);
        
        GetComponent<MeshCollider>().enabled = true;

        // Remove the Parent
        transform.parent = null;

        // Hide the Canvas
        gunCanvas.enabled = false;

        // Reset Aiming
        isAiming = false;
        currentDeviationX = GetShotDeviation().x;
        currentDeviationY = GetShotDeviation().y;

        // Resizing
        //transform.localScale = Vector3.one;
    }

    /// I_Interactable
    public FPS_InteractionType GetInteraction()
    {
        return FPS_InteractionType.Equippable;
    }
    
    // #########################################################
    // # ---------------- ENUMERABLE METHODS ----------------- #
    // #########################################################

    protected IEnumerator ShotEffect()
    {

        line.enabled = true;

        yield return shotDuration;

        line.enabled = false;

    }

    // #########################################################
    // # ----------------- GETTERS | SETTERS ----------------- #
    // #########################################################

    // --- GUN VARIABLES --- 
    public abstract string GetWeaponName();
    public abstract float GetDamage();
    public abstract float GetRange();
    public abstract float GetReloadTime();
    public abstract float GetReloadTimer();
    public abstract float GetRPM();
    public abstract float GetRPS();
    public abstract Vector2 GetShotDeviation();
    public abstract float GetRecoil();
    public abstract float GetAimedSpread();
    //public DamageType GetDamageType() { return DamageType.Ballistic; }
    public abstract FPS_TriggerType GetTriggerType();


    public abstract Vector3 GetHandPosition();
    public abstract Vector3 GetAimedPosition();
    
   
    // --- AMMO RESERVE --- 
    public abstract int GetClipSize();
    public abstract int GetMaxAmmo();
    public abstract int GetAmmo();
    public abstract int GetCurrentClip();
    public abstract bool IsReloading();

    // #########################################################
    // # ------------------------ EOF ------------------------ #
    // #########################################################
}
