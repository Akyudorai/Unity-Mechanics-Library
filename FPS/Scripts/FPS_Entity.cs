using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Entity : MonoBehaviour {
    
    [Header("Entity Health")]
    [SerializeField] protected float maxHealth = 100.0f;
    protected float health;
    [SerializeField] [Range(0.0f, 1.0f)] protected float setHealth = 1.0f;
    [SerializeField] bool isKillable = false;

    public float GetCurrentHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

    [SerializeField] List<GameObject> criticalPoints = new List<GameObject>();

    protected virtual void Destruct()
    {
        // Temporary -- Destruct script should be found in the child script
        Destroy(gameObject);
    }

    public void DamageEntity(float amount, GameObject hit)
    { 

        // If it's a critical hit location, multiply the damage by ten.
        if (criticalPoints.Contains(hit)) { 
            health -= amount * 50.0f;
        }

        else { health -= amount; }
        
        if (!isKillable && health - amount < 1)
        {
            health = 1;
        }
    }
}
