using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsion_Arrow_v1 : Projectile {

    [SerializeField] private float blastForce = 5.0f;
    [SerializeField] private float blastRadius = 7.0f;

    [SerializeField] private GameObject blastPrefab;

    
    private void Update()
    {
        if (isActive)
        {
            if (rigid.constraints != RigidbodyConstraints.FreezeAll && rigid.velocity != Vector3.zero)
                transform.forward = rigid.velocity;
        }        
    }

    public override void Launch(Vector3 direction, float power, ForceMode forceMode, bool delayCollision)
    {
        if (delayCollision)
            StartCoroutine(DelayCollision());

        else
            GetComponent<BoxCollider>().isTrigger = false;


        rigid.AddForce(direction * (projectileSpeed * power), ForceMode.Impulse);
        rigid.constraints = RigidbodyConstraints.None;

        isActive = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Deal Damage
        if (collision.gameObject.GetComponentInParent<FPS_Entity>() != null)
        {
            collision.gameObject.GetComponentInParent<FPS_Entity>().DamageEntity(50.0f, collision.gameObject);
            
        }

        // Explode, dealing damage in a radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider c in objectsInRange)
        {
            // Linear Falloff
            float proximity = (transform.position - c.transform.position).magnitude;
            float effect = (proximity / blastRadius);

            if (effect < 0.25f) // Blast falloff decays down to a maximum of 25% of the base damage
                effect = 0.25f;

            // If the target hit has an entity script, damage it.
            if (c.GetComponent<FPS_Entity>() != null)
                c.GetComponent<FPS_Entity>().DamageEntity(50 * effect, collision.gameObject);

            // If it has a rigidbody, apply a blast force to it
            if (c.GetComponent<Rigidbody>() != null)
                c.GetComponent<Rigidbody>().AddExplosionForce(blastForce, collision.contacts[0].point, blastRadius, 1.0f, ForceMode.Impulse);
        }

        Instantiate(blastPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
}
