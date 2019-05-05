using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    protected Rigidbody rigid;
    
    [SerializeField] protected float projectileSpeed;

    protected bool isActive;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Launch(Vector3 direction, float scale, ForceMode forceMode, bool delayCollision) { }

    protected IEnumerator DelayCollision()
    {
        yield return new WaitForSeconds(0.15f);
        
        GetComponent<BoxCollider>().isTrigger = false;
    }
}
