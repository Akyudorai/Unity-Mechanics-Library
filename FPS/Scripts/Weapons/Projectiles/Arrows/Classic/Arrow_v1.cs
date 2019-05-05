using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_v1 : Projectile {

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

        if (!collision.gameObject.GetComponentInParent<Projectile>())
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = collision.transform;
            transform.parent = emptyObject.transform;
            //transform.parent = collision.transform;

            //transform.localRotation = Quaternion.LookRotation(-collision.contacts[0].normal);

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;



            // Deal Damage
            if (collision.gameObject.GetComponentInParent<FPS_Entity>() != null)
            {
                collision.gameObject.GetComponentInParent<FPS_Entity>().DamageEntity(100.0f, collision.gameObject);
            }

            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
