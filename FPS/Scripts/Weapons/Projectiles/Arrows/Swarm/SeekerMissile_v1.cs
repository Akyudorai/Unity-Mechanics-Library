using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile_v1 : MonoBehaviour {

    public GameObject target;

    [SerializeField] float missileSpeed = 15.0f;
    [SerializeField] GameObject seekerBlastFX;

    private float angularRate = 0.0f;
    private float speed = 0.0f;

    private bool accelerate = false;

    private void Start()
    {
        if (target == null)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

            if (targets.Length > 0)
            {
                foreach (GameObject o in targets)
                {
                    if (target != null)
                    {
                        // If the target is closer, set that as the new target.
                        if (Vector3.Distance(o.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position))
                        {
                            target = o;
                        }
                    }

                    // If no target exists, set it as the active target;
                    else
                    {
                        target = o;
                    }
                }


            }

        }

        StartCoroutine(Speed());
    }

    private void Update()
    {

        if (target != null)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime * angularRate);

        }

        angularRate += Time.deltaTime;

        if (accelerate && speed < missileSpeed * 2)
        {
            speed += Time.deltaTime * missileSpeed;
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        if (target == null)
        {
            //Destroy(gameObject);
        }

    }

    IEnumerator Speed()
    {
        speed = missileSpeed / 2;

        yield return new WaitForSeconds(1.5f);

        accelerate = true;

    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == target)
        {
            Debug.Log("TargetHit");
            Instantiate(seekerBlastFX, transform.position, Quaternion.identity);

            if (target.GetComponent<FPS_Entity>() != null)
            {
                target.GetComponent<FPS_Entity>().DamageEntity(10.0f, col.gameObject);
            }

            Destroy(gameObject);
        }

        else
        {
            Instantiate(seekerBlastFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


    }

}
