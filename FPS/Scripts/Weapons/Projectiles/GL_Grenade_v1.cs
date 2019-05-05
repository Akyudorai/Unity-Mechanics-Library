using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GL_Grenade_v1 : MonoBehaviour {

    // ########################################################
    // # ----------------- WEAPON COMPONENTS ---------------- #
    // ########################################################

    [SerializeField] GameObject explosion;
    [SerializeField] bool Debug_Radius = false;
    [SerializeField] float decayTime = 10.0f; // Time before the grenade despawns
    
    Rigidbody rigid;

    // ########################################################
    // # ----------------- WEAPON VARIABLES ----------------- #
    // ########################################################

    const float directDamage = 150.0f;
    const float splashDamage = 100.0f;
    
    [SerializeField] float explosionRadius = 7.0f;

    // ########################################################
    // # ------------------ INITIALIZATION ------------------ #
    // ########################################################

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    // ########################################################
    // # ------------------- METHODOLOGY -------------------- #
    // ########################################################

    private void Update()
    {
        //rigid.velocity = -transform.forward * projectileVelocity;

        if (decayTime > 0)
            decayTime -= Time.deltaTime;

        if (decayTime <= 0)
        {
            GameObject o = Instantiate(explosion, transform.position, Quaternion.identity);
            o.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, explosionRadius * 2); // Takes in Diameter
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        // ########################################################
        // # ------------------ DIRECT DAMAGE ------------------- #
        // ########################################################

        // If the target hit has an entity script, damage it.
        if (col.gameObject.GetComponentInParent<FPS_Entity>() != null)
            col.gameObject.GetComponentInParent<FPS_Entity>().DamageEntity(directDamage, col.gameObject);

        // ########################################################
        // # ------------------ SPLASH DAMAGE ------------------- #
        // ########################################################

        // Explode, dealing damage in a radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider c in objectsInRange)
        {
            // Linear Falloff
            float proximity = (transform.position - c.transform.position).magnitude;
            float effect = (proximity / explosionRadius);

            if (effect < 0.25f) // Blast falloff decays down to a maximum of 25% of the base damage
                effect = 0.25f;

            // If the target hit has an entity script, damage it.
            if (c.GetComponent<FPS_Entity>() != null)
                c.GetComponent<FPS_Entity>().DamageEntity(splashDamage * effect, col.gameObject);

            // If it has a rigidbody, apply a blast force to it
            if (c.GetComponent<Rigidbody>() != null)
                c.GetComponent<Rigidbody>().AddExplosionForce(5.0f, col.contacts[0].point, explosionRadius, 1.0f, ForceMode.Impulse);
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }

    // ########################################################
    // # ------------------ DEBUG RADIUS -------------------- #
    // ########################################################

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;

        if (Debug_Radius)
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }

    // #########################################################
    // # ------------------------ EOF ------------------------ #
    // #########################################################

}

