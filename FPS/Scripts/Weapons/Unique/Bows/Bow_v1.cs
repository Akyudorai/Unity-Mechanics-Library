using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_v1 : Weapon {

    // ########################################################
    // # ----------------- WEAPON COMPONENTS ---------------- #
    // ########################################################

    [SerializeField] LineRenderer bowstring;
    [SerializeField] GameObject stringStart;
    [SerializeField] GameObject stringBend;
    [SerializeField] GameObject stringEnd;
    [SerializeField] readonly GameObject arrowObj;

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const string weaponName = "devBow";
    const float damage = 100.0f;
    const float range = 0;
    const float reloadTime = 0; // 1.6s Reload Time
    const float rps = 0; // 5.0/s || 300/rpm;
    const int clipSize = 1;
    const int maxAmmo = 60;

    float shotDeviationX = 0.26f;
    float shotDeviationY = 0.26f;

    private Vector3 handPosition = new Vector3(0.35f, -0.71f, 1.316f);
    private Vector3 aimedPosition = new Vector3(0.089f, -0.387f, 0.91f);


    private bool shotReleased = false;
    //private Vector3 releasePoint;

    [SerializeField] private float drawPower = 0.0f;

    private GameObject dockedArrow = null;

    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    public override void PrimaryFunction()
    {
        // Render String
        bowstring.SetPosition(0, stringStart.transform.position);
        bowstring.SetPosition(1, stringBend.transform.position);
        bowstring.SetPosition(2, stringEnd.transform.position);

        // Draw Power
        drawPower = stringBend.transform.localPosition.y;
        currentDeviationX = shotDeviationX - (drawPower / 4);
        currentDeviationY = shotDeviationY - (drawPower / 4);

        if (dockedArrow == null && stringBend.transform.localPosition == Vector3.zero)
        {
            dockedArrow = Instantiate(arrowObj, stringBend.transform.position - transform.right * 0.1f, Quaternion.LookRotation(-transform.up));
            dockedArrow.transform.parent = stringBend.transform;
        }

        if (!shotReleased)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (stringBend.transform.localPosition.y < 1.0f)
                    stringBend.transform.position += transform.up * Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (dockedArrow != null)
                {
                    dockedArrow.transform.parent = null;
                    dockedArrow.GetComponent<Projectile>().Launch(dockedArrow.transform.forward, drawPower, ForceMode.Impulse, false);
                }

                //releasePoint = stringBend.transform.localPosition;
                shotReleased = true;
                dockedArrow = null;

            }
        }

        else
        {
            if (stringBend.transform.localPosition != Vector3.zero)
            {
                stringBend.transform.localPosition = Vector3.Lerp(stringBend.transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * 10);
            }

            else if (stringBend.transform.localPosition.y <= 0.15f)
            {
                stringBend.transform.localPosition = Vector3.zero;
                shotReleased = false;
            }
        }
    }

    public override void SecondaryFunction()
    {
        
    }

    public override void Interact()
    {
        base.Interact();

        bowstring.enabled = true;
    }

    public override void Release()
    {
        base.Release();

        Destroy(dockedArrow);
        dockedArrow = null;
        bowstring.enabled = false;
    }

    // #########################################################
    // # ----------------- GETTERS | SETTERS ----------------- #
    // #########################################################

    // --- GUN VARIABLES --- 
    public override string GetWeaponName() { return weaponName; }
    public override float GetDamage() { return damage; }
    public override float GetRange() { return range; }
    public override float GetReloadTime() { return reloadTime; }
    public override float GetReloadTimer() { return reloadTimer; }
    public override float GetRPM() { return 0; }
    public override float GetRPS() { return 0; }
    public override float GetRecoil() { return 0; }
    //public override float GetMaxSpread() { return shotDeviationX * 2; }
    public override float GetAimedSpread() { return shotDeviationX / 5; }
    //public DamageType GetDamageType() { return DamageType.Ballistic; }
    public override FPS_TriggerType GetTriggerType() { return FPS_TriggerType.Charged; }

    public override Vector2 GetShotDeviation() { return new Vector2(shotDeviationX, shotDeviationY); }
    public override Vector3 GetHandPosition() { return handPosition; }
    public override Vector3 GetAimedPosition() { return aimedPosition; }
    public override Vector3 GetWeaponScale() { return new Vector3(0.65f, 0.8f, 0.65f); }

    // --- AMMO RESERVE --- 
    public override int GetClipSize() { return clipSize; }
    public override int GetMaxAmmo() { return maxAmmo; }
    public override int GetAmmo() { return ammo; }
    public override int GetCurrentClip() { return currentClip; }
    public override bool IsReloading() { return isReloading; }

    // #########################################################
    // # ------------------------ EOF ------------------------ #
    // #########################################################



}
