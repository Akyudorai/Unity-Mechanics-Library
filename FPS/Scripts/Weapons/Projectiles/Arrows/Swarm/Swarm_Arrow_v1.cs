using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_Arrow_v1 : Projectile {
    
    [SerializeField] int numberOfMissiles;
    [SerializeField] GameObject seekerMissileObj;

    private float timer = 1.5f;

    private void Update()
    {
        if (isActive)
        {
            if (rigid.constraints != RigidbodyConstraints.FreezeAll && rigid.velocity != Vector3.zero)
                transform.forward = rigid.velocity;

            if (timer <= 0)
            {
                for (int j = -5; j < 5; j++)
                {
                    for (int i = -5; i < 5; i++)
                    {
                        Vector3 lookDir = transform.forward;
                        lookDir.x -= 0.5f;
                        lookDir.y += 2.5f - (i * 0.1f);

                        Vector3 position = transform.position;
                        position.x += (j % 2) + i;
                        position.z += (i % 3) + j;


                        GameObject seeker = Instantiate(seekerMissileObj, position, Quaternion.LookRotation(lookDir));                        
                    }
                }

                Destroy(gameObject);
                
            }

            if (timer > 0)
                timer -= Time.deltaTime;
            
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
            isActive = false;
        }
    }
}
