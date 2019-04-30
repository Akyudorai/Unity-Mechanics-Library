using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_v1 : Weapon {

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const string weaponName = "devShotgun";
    const float damage = 15.0f;
    const float range = 50.0f;
    const float reloadTime = 0.4f; // 3.2s Reload Time (0.4 * 8 shells)
    const float rps = 1.0f; // 1.0/s || 60/rpm;
    const int clipSize = 8;
    const int maxAmmo = 64;
    const float spread = 0.25f; // UNIQUE TO SHOTGUN -- The width of the bullet spread
    const float recoil = 0.5f;

    float shotDeviationX = 0.05f;
    float shotDeviationY = 0.05f;

    private Vector3 handPosition = new Vector3(0.67f, -0.447f, 0.729f);
    private Vector3 aimedPosition = new Vector3(0.089f, -0.387f, 0.91f);

    // Unique to shotgun
    [SerializeField] LineRenderer[] lines = new LineRenderer[8];


    // #########################################################
    // # ---------------- OVERRIDABLE METHODS ---------------- #
    // #########################################################

    override public void PrimaryFunction()
    {
        if (nextShot <= 0 && currentClip > 0 && !isReloading)
        {
            // Stop reloading if currently reloading -- UNIQUE TO SHOT LOADING MECHANISMS
            StopAllCoroutines();

            for (int i = 0; i < lines.Length; i++)
            {
                // Animate Shot
                StartCoroutine(ShotEffect(i));

                // Hitcast Shot
                Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                lines[i].SetPosition(0, originPoint.position);

                float randX = Random.Range(-currentDeviationX, currentDeviationX);
                float randY = Random.Range(-currentDeviationY, currentDeviationY);
                Vector3 direction = cam.transform.forward + new Vector3(0, randY, randX);

                if (Physics.Raycast(rayOrigin, direction, out hit, range))
                {
                    // Declare Target
                    GameObject targetHit = hit.transform.gameObject;

                    // Set hit Point
                    lines[i].SetPosition(1, hit.point);
                    GameObject impact = Instantiate(bullethole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                    impact.transform.parent = targetHit.transform;

                }

                else
                    lines[i].SetPosition(1, rayOrigin + (direction * range));

            }

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
                if (currentDeviationX < shotDeviationX) currentDeviationX += recoil / 1.5f;
                if (currentDeviationY < shotDeviationY) currentDeviationY += recoil / 1.5f;
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
            currentDeviationX = shotDeviationX / 1.5f;
            currentDeviationY = shotDeviationY / 1.5f;
        }

        else if (!isAiming)
        {
            currentDeviationX = shotDeviationX;
            currentDeviationY = shotDeviationY;
        }
    }

    // #########################################################
    // # ---------------- ENUMERABLE METHODS ----------------- #
    // #########################################################

    // Unique to Shotgun -- Multiple Line Renders
    IEnumerator ShotEffect(int index)
    {

        lines[index].enabled = true;

        yield return shotDuration;

        lines[index].enabled = false;

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
    public override float GetAimedSpread() { return shotDeviationX / 1.5f; }
    //public DamageType GetDamageType() { return DamageType.Ballistic; }
    public override FPS_TriggerType GetTriggerType() { return FPS_TriggerType.SemiAuto; }

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
