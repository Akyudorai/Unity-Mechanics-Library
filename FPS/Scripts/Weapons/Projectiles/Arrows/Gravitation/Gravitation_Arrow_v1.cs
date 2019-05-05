using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitation_Arrow_v1 : Projectile {

    [SerializeField] private float pullForce = 5.0f;
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

        

        Instantiate(blastPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
}
