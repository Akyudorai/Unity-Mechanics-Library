using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_v1 : MonoBehaviour {

    public void OnCollisionEnter(Collision collision)
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
