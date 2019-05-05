using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadier_v1 : Weapon {

    // ########################################################
    // # ----------------- WEAPON COMPONENTS ---------------- #
    // ########################################################
    
    [SerializeField] GameObject grenadeObj;

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const string weaponName = "devGrenadier";
    const float damage = 100.0f;
    const float range = 0;
    const float reloadTime = 1.6f; // 1.6s Reload Time
    const float rps = 2.0f; // 5.0/s || 300/rpm;
    const int clipSize = 8;
    const int maxAmmo = 64;
    const float recoil = 0.1f;

    float shotDeviationX = 0.025f;
    float shotDeviationY = 0.025f;

    private Vector3 handPosition = new Vector3(1.225f, -0.735f, 0.899f);
    private Vector3 aimedPosition = new Vector3(0.089f, -0.387f, 0.91f);

    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    public override void PrimaryFunction()
    {
        if (nextShot <= 0 && currentClip > 0 && !isReloading)
        {
            float randX = Random.Range(-currentDeviationX, currentDeviationX);
            float randY = Random.Range(-currentDeviationY, currentDeviationY);
            Vector3 direction = cam.transform.forward + new Vector3(0, randY, randX);

            // Fire
            GameObject nade = Instantiate(grenadeObj, originPoint.transform.position, Quaternion.LookRotation(direction));
            nade.GetComponent<Rigidbody>().useGravity = true;
            nade.GetComponent<Rigidbody>().AddForce(-transform.up * 25, ForceMode.Impulse);

            // RECOIL
            if (!isAiming)
            {
                // Can only increase recoil if it's less than double the original value
                if (currentDeviationX < shotDeviationX * 2) currentDeviationX += recoil;
                if (currentDeviationY < shotDeviationY * 2) currentDeviationY += recoil;

            }

            else if (isAiming)
            {
                // Can only increase recoil if it's less than double the original value
                if (currentDeviationX < shotDeviationX) currentDeviationX += recoil;
                if (currentDeviationY < shotDeviationY) currentDeviationY += recoil;
            }

            nextShot = (1 / rps);
            currentClip--;
        }

        else if (nextShot <= 0 && currentClip <= 0)
        {
            Reload();
        }
    }

    public override void SecondaryFunction()
    {

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
    public override float GetRPM() { return rps * 60; }
    public override float GetRPS() { return rps; }
    public override float GetRecoil() { return recoil; }
    //public override float GetMaxSpread() { return shotDeviationX * 2; }
    public override float GetAimedSpread() { return shotDeviationX / 5; }
    //public DamageType GetDamageType() { return DamageType.Ballistic; }
    public override FPS_TriggerType GetTriggerType() { return FPS_TriggerType.SemiAuto; }

    public override Vector2 GetShotDeviation() { return new Vector2(shotDeviationX, shotDeviationY); }
    public override Vector3 GetHandPosition() { return handPosition; }
    public override Vector3 GetAimedPosition() { return aimedPosition; }
    public override Vector3 GetWeaponScale() { return new Vector3(0.75f, 0.65f, 0.65f); }

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
