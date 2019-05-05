using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce_Arrow_v1 : Projectile {
    
    [SerializeField] int maxBounces = 2;
    int totalBounces;
 
    private void Start()
    {
        totalBounces = 0;        
    }

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
            collision.gameObject.GetComponentInParent<FPS_Entity>().DamageEntity(100.0f, collision.gameObject);
            totalBounces = maxBounces;
        }

        // Bouncing Behaviour
        if (totalBounces < maxBounces)
        {
            Vector3 reflectDir = Vector3.Reflect(rigid.velocity, collision.contacts[0].normal);

            rigid.velocity = reflectDir;
            //transform.LookAt();
            
        }

        else
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = collision.transform;
            transform.parent = emptyObject.transform;

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            GetComponent<BoxCollider>().isTrigger = true;
        }

        Debug.Log("Bouncing Off: " + collision.gameObject.name);
        totalBounces++;
    }
}
