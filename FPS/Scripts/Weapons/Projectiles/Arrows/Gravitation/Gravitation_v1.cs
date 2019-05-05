using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitation_v1 : MonoBehaviour {

    [SerializeField] private float pullForce = 5.0f;
    [SerializeField] private float blastRadius = 7.0f;

    // Consistant Gravitation
    private void Update()
    {
        // Explode, dealing damage in a radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider c in objectsInRange)
        {
            // Linear Falloff
            float proximity = (transform.position - c.transform.position).magnitude;
            float effect = (proximity / blastRadius);

            // If it has a rigidbody, apply a blast force to it
            if (c.GetComponent<Rigidbody>() != null)
                c.GetComponent<Rigidbody>().AddExplosionForce(-pullForce, transform.position, blastRadius, 0, ForceMode.Impulse);
        }
    
}
}
