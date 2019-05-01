using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AR_v1 : Weapon {

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const string weaponName = "devAR";
    const float damage = 5.0f;
    const float range = 100.0f;
    const float reloadTime = 2.06f; // 1.6s Reload Time
    const float rps = 10.0f; // 10.0/s || 600/rpm;
    const int clipSize = 100;
    const int maxAmmo = 600;
    const float recoil = 0.025f;

    float shotDeviationX = 0.05f;
    float shotDeviationY = 0.05f;

    Vector3 handPosition = new Vector3(0.641f, -0.462f, 1.6f);
    Vector3 aimedPosition = new Vector3(0.004f, -0.325f, 1.6f);


    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    override public void PrimaryFunction()
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
                GameObject impact = Instantiate(bullethole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                impact.transform.parent = targetHit.transform;

                // If the target is an entity, damage its health script
                if (targetHit.GetComponentInParent<FPS_Entity>() != null)
                    targetHit.GetComponentInParent<FPS_Entity>().DamageEntity(damage, targetHit);
            }

            else
                line.SetPosition(1, rayOrigin + (cam.transform.forward * range));


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
                if (currentDeviationX < shotDeviationX) currentDeviationX += recoil / 2;
                if (currentDeviationY < shotDeviationY) currentDeviationY += recoil / 2;
            }
            
            nextShot = (1 / rps);
            currentClip--;
        }

        else if (nextShot <= 0 && currentClip <= 0)
        {
            Reload();
        }

    }

    override public void SecondaryFunction()
    {
        isAiming = !isAiming;

        if (isAiming)
        {
            currentDeviationX = GetAimedSpread();
            currentDeviationY = GetAimedSpread();

            cam.fieldOfView = 45.0f;
        }

        else if (!isAiming)
        {
            currentDeviationX = shotDeviationX;
            currentDeviationY = shotDeviationY;

            cam.fieldOfView = 60.0f;
        }

    }

    public override void Release()
    {
        base.Release();

        cam.fieldOfView = 60.0f;
        isAiming = false;
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
    public override FPS_TriggerType GetTriggerType() { return FPS_TriggerType.Automatic; }

    public override Vector2 GetShotDeviation() { return new Vector2(shotDeviationX, shotDeviationY); }
    public override Vector3 GetHandPosition() { return handPosition; }
    public override Vector3 GetAimedPosition() { return aimedPosition; }


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
