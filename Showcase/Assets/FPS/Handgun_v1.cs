using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Handgun_v1 : MonoBehaviour {

    // ########################################################
    // # ----------------- WEAPON COMPONENTS ---------------- #
    // ########################################################

    Camera cam;
    [SerializeField] Transform originPoint;
    LineRenderer line;
    
    [Header("GUI")]
    public Canvas gunCanvas;
    [SerializeField] Text clipInfo;
    [SerializeField] Text ammoReserve;
    [SerializeField] Image reloadImg;

    [SerializeField] GameObject bullethole;

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const string weaponName = "devPistol";
    const float damage = 10.0f;
    const float range = 30.0f;
    const float reloadTime = 1.6f; // 1.6s Reload Time
    const float rps = 5.0f; // 5.0/s || 300/rpm;
    const int clipSize = 12;
    const int maxAmmo = 120;

    int currentClip; // Number of Shots in the clip
    int ammo; // Total Number of Shots in reserve
    bool isReloading; // State-based Boolean
    float reloadTimer;

    float shotDeviationX = 0.05f;
    float shotDeviationY = 0.05f;

    public float currentDeviationX;
    public float currentDeviationY;
    public bool isAiming;

    public Vector3 handPosition = new Vector3(0.67f, -0.447f, 0.729f);
    public Vector3 aimedPosition = new Vector3(0.089f, -0.387f, 0.91f);

    WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    float nextShot;

    // ########################################################
    // # ------------------ INITIALIZATION ------------------ #
    // ########################################################

    private void Awake()
    {
        if (cam == null)
            cam = GetComponentInParent<Camera>();

        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        currentClip = clipSize; // 12 Shots
        ammo = maxAmmo; // 120 Reserve
        isReloading = false;
        reloadTimer = 0; // Current Reload Timer;

        currentDeviationX = shotDeviationX;
        currentDeviationY = shotDeviationY;
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

        GUI();
    }

    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    public void PrimaryFunction()
    {
        if (nextShot <= 0 && currentClip > 0 && !isReloading)
        {
            // Animate Shot
            StartCoroutine(ShotEffect());

            // Hitcast Shot
            
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            line.SetPosition(0, originPoint.position);

            float randX = Random.Range(-currentDeviationX, currentDeviationX);
            float randY = Random.Range(-currentDeviationY, currentDeviationY);
            Vector3 direction = cam.transform.forward + new Vector3(0, randY, randX);

            if (Physics.Raycast(rayOrigin, direction, out hit, range))
            {
                // Declare Target
                GameObject targetHit = hit.transform.gameObject;

                // Set hit Point
                line.SetPosition(1, hit.point);
                Instantiate(bullethole, hit.point, Quaternion.LookRotation(hit.normal));

                // If the target is an entity, damage its health script
                //if (targetHit.GetComponent<Entity>() != null)
                    //targetHit.GetComponent<Entity>().DamageEntity(damage);
            }

            else
                line.SetPosition(1, rayOrigin + (cam.transform.forward * range));

            nextShot = (1 / rps);
            currentClip--;
        }

        else if (nextShot <= 0 && currentClip <= 0)
        {
            Reload();
        }

    }

    public void SecondaryFunction()
    {
        isAiming = !isAiming;

        if (isAiming)
        {
            currentDeviationX = shotDeviationX / 3;
            currentDeviationY = shotDeviationY / 3;
        }

        else if (!isAiming)
        {
            currentDeviationX = shotDeviationX;
            currentDeviationY = shotDeviationY;
        }



    }

    public void Reload()
    {
        StartCoroutine(I_Reload());
    }

    private IEnumerator I_Reload()
    {
        isReloading = true;
        reloadTimer = reloadTime;

        yield return new WaitForSeconds(reloadTime);

        int refill = clipSize - currentClip;

        if (ammo - refill >= 0)
        {
            currentClip = clipSize;
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
    // # ---------------- ENUMERABLE METHODS ----------------- #
    // #########################################################

    IEnumerator ShotEffect()
    {

        line.enabled = true;

        yield return shotDuration;

        line.enabled = false;

    }

    // #########################################################
    // # ----------------- GETTERS | SETTERS ----------------- #
    // #########################################################

    // --- GUN VARIABLES --- 
    public string GetWeaponName() { return weaponName; }
    public float GetDamage() { return damage; }
    public float GetRange() { return range; }
    public float GetReloadTime() { return reloadTime; }
    public float GetReloadTimer() { return reloadTimer; }
    public float GetRPM() { return rps * 60; }
    public float GetRPS() { return rps; }
    //public DamageType GetDamageType() { return DamageType.Ballistic; }
    //public TriggerType GetTriggerType() { return TriggerType.SemiAuto; }


    // --- AMMO RESERVE --- 
    public int GetClipSize() { return clipSize; }
    public int GetMaxAmmo() { return maxAmmo; }
    public int GetAmmo() { return ammo; }
    public int GetCurrentClip() { return currentClip; }
    public bool IsReloading() { return isReloading; }

    // #########################################################
    // # ------------------------ EOF ------------------------ #
    // #########################################################
}
