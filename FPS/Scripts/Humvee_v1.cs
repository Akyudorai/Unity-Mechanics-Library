using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humvee_v1 : FPS_Entity {

    [SerializeField] GameObject[] meshObjects;
    [SerializeField] GameObject smokeEmitter;

    private void Start()
    {
        health = maxHealth * setHealth;
    }

    private void Update()
    {
        if (health / maxHealth < 0.25f)
        {
            smokeEmitter.SetActive(true);
        }

        if (health <= 0)
        {
            Destruct();
        }
    }

    override protected void Destruct()
    {
        foreach (GameObject o in meshObjects)
        {
            //o.transform.parent = null;
            o.GetComponent<Rigidbody>().isKinematic = false;
            
            o.GetComponent<Rigidbody>().AddExplosionForce(5, transform.localPosition - transform.up * 5 + transform.forward, 5.0f, 2.0f, ForceMode.Impulse);
            o.GetComponent<MeshCollider>().convex = true;

            o.transform.parent = null;
            //o.GetComponent<Rigidbody>().AddForceAtPosition(transform.up * Random.Range(25, 50), transform.localPosition, ForceMode.Impulse);
        }
    }
	
}
