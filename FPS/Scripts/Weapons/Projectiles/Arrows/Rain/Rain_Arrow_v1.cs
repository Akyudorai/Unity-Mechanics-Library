using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_Arrow_v1 : Projectile {

    [SerializeField] GameObject classicArrow;

    private Vector3 startPos;
    private float timer = 2.0f;
    //private bool collision = false;

    private void Start()
    {
       // for (int j = -5; j < 5; j++)
       // {
       //     for (int i = -5; i < 5; i++)
       //     {
       //         Vector3 lookDir = transform.forward;
       //         lookDir.x += 0.5f;
       //         lookDir.y -= 2.5f - (i * 0.1f);

       //         Vector3 position = transform.position;
       //         position.x += (j % 2) + i;
       //         position.z += (i % 3) + j;


       //         GameObject arrow = Instantiate(classicArrow, position, Quaternion.LookRotation(lookDir));
       //         arrow.GetComponent<Projectile>().Launch(arrow.transform.forward + (rigid.velocity * 0.01f), 0.1f, ForceMode.Impulse, true);
       //     }
       // }

       //Destroy(gameObject);
       
    }

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
                        lookDir.x += 0.5f;
                        lookDir.y -= 2.5f - (i * 0.1f);

                        Vector3 position = transform.position;
                        position.x += (j % 2) + i;
                        position.z += (i % 3) + j;


                        GameObject arrow = Instantiate(classicArrow, position, Quaternion.LookRotation(lookDir));
                        arrow.GetComponent<Projectile>().Launch(arrow.transform.forward + (rigid.velocity * 0.01f), 0.1f, ForceMode.Impulse, true);
                    }
                }

                Destroy(gameObject);
            }

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

        startPos = transform.position;
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

            Debug.Log("Collision");
            isActive = false;
        }
        
    }

    
}
