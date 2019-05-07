using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RPG3_Warrior : RPG3_Class {

    // Warrior Stats

    // Warrior Abilities

    private void Awake()
    {
        abilities.Add(CreateInstance<RPG3_Rend>());
        abilities.Add(CreateInstance<RPG3_Charge>());
    }        
}

// Deal Damage
public class RPG3_Rend : RPG3_Ability
{
    float damage = 15.0f;

    public override string GetName() { return "Rend"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Ax_2"); }

    // Requirements:
    //      Target || target position
    //      Ability Mode [ targeted | directional/position | selfcast ]  
    //            
    //
    //


    public override void Activate(RPG3_AbilitySettings settings)
    {
        // Do a thing;
        Debug.Log("Rend Activated");

        // Check if owner is within distance of the target || MELEE ATTACK
        if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < 5.0f)
        {
            settings.targetEntity.DamageEntity(damage, null);
            settings.owner.DelayGCD();
        }
        
        else
        {
            Debug.Log("Out of Range");
        }
    }
}


// Move to target entity/position
public class RPG3_Charge : RPG3_Ability
{
    public override string GetName() { return "Charge"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Helmet_4"); }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        // Do a thing;
        Debug.Log("Charge Activated");

        // Check if owner is within distance of the target
        if (Vector3.Distance(settings.owner.transform.position, settings.targetEntity.transform.position) < 20.0f)
        {
            if (!settings.owner.disableControls)
            {
                settings.owner.StartCoroutine(settings.owner.MoveToEntity(settings.targetEntity));
                settings.owner.disableControls = true;
                settings.owner.DelayGCD();
            }
  
        }

        else
        {
            Debug.Log("Out of Range");
        }
    }   
}


