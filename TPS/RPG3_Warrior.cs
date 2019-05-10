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

        resourceType = RPG3_ResourceType.Rage;        
        resourcePerSecond = -1;

    }   
}

// Deal Damage
public class RPG3_Rend : RPG3_Ability
{
    float damage = 15.0f;
    float cooldown = 0;

    public override string GetName() { return "Rend"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Ax_2"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (settings.targetEntity == null)
        {
            Debug.Log("No Target Selected");
            return;
        }

        if (settings.target == settings.owner)
        {
            Debug.Log("Can't target yourself with that ability");
            return;
        }

        // Cost = 15 resource
        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().GetCurrentResource() > 15.0f)
        {
            // Check if owner is within distance of the target || MELEE ATTACK
            if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < 5.0f)
            {
                Effect(settings);

                settings.owner.GetComponent<TPSC_RPG1>().GetEntity().SubtractResource(15);

                settings.owner.DelayGCD();
                cdTimer = cooldown;
                
            }

            else
            {
                Debug.Log("Out of Range");
            }
        }
        
    }

    public override void Effect(RPG3_AbilitySettings settings)
    {
        
        settings.targetEntity.DamageEntity(damage, null);
    }

   
}


// Move to target entity/position
public class RPG3_Charge : RPG3_Ability
{
    float cooldown = 5.0f;

    public override string GetName() { return "Charge"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Helmet_4"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (settings.targetEntity == null)
        {
            Debug.Log("No Target Selected");
            return;
        }

        if (settings.target == settings.owner)
        {
            Debug.Log("Can't target yourself with that ability");
            return;
        }

        // Check if owner is within distance of the target
        if (Vector3.Distance(settings.owner.transform.position, settings.targetEntity.transform.position) < 20.0f)
        {
            if (!settings.owner.disableControls)
            {
               Effect(settings);

                cdTimer = cooldown;
                settings.owner.DelayGCD();                
            }
  
        }

        else
        {
            Debug.Log("Out of Range");
        }
    }

    public override void Effect(RPG3_AbilitySettings settings)
    {        
        settings.owner.disableControls = true;
        settings.owner.StartCoroutine(settings.owner.MoveToEntity(settings.targetEntity));
        

        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().resource + 40 <= 100)
            settings.owner.GetComponent<TPSC_RPG1>().GetEntity().AddResource(40);
        else
            settings.owner.GetComponent<TPSC_RPG1>().GetEntity().resource = 100;
    }    
}


