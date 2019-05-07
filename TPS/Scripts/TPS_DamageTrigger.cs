using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_DamageTrigger : MonoBehaviour {

    [SerializeField] float damage;

    float timer = 1.0f;

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider col)
    {        
        if (col.gameObject.GetComponentInParent<TPS_Entity>())
        {            
            if (timer <= 0)
            {
                col.gameObject.GetComponentInParent<TPS_Entity>().DamageEntity(damage, col.gameObject);
                timer = 1.0f;
            }
            
        }
    }
}
