using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG3_Rogue : RPG3_Class {

    // Rogue Stats

    // Rogue Abilities

    private void Awake()
    {
        abilities.Add(CreateInstance<RPG3_Shiv>());
        abilities.Add(CreateInstance<RPG3_Eviscerate>());
       
       
        resourceType = RPG3_ResourceType.Energy;
        resourcePerSecond = 10;

    }

    
}

public class RPG3_Shiv : RPG3_Ability
{
    float damage = 12.0f;
    int cost = 45;
    float range = 5.0f;
    float cooldown = 0;
    
    public override string GetName() { return "Shiv"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Sword_2"); }

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
        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().GetCurrentResource() > cost)
        {
            // Check if owner is within distance of the target || MELEE ATTACK, 5 units
            if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < range)
            {
                Effect(settings);
                
                if (settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel < 5)
                    settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel++;        
                
                Debug.Log(settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel);
                
                settings.owner.GetComponent<TPSC_RPG1>().GetEntity().SubtractResource(cost);

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

public class RPG3_Eviscerate : RPG3_Ability
{
    float damage = 10.0f;
    int cost = 40;
    float range = 5.0f;
    float cooldown = 0;

    public override string GetName() { return "Eviscerate"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Ax_1"); }

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

        if (settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel < 1)
        {
            Debug.Log("Need combo points to activate this ability");
            return;
        }

        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().GetCurrentResource() > cost)
        {
            // Check if owner is within distance of the target || MELEE ATTACK, 5 units
            if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < range)
            {
                Effect(settings);
                
                settings.owner.GetComponent<TPSC_RPG1>().GetEntity().SubtractResource(cost);

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
        float totalDamage = damage + (15 * settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel);

        Debug.Log(totalDamage);

        settings.targetEntity.DamageEntity(totalDamage, null);
        settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel = 0;
    }
}

