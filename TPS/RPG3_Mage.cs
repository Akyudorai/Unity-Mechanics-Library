using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG3_Mage : RPG3_Class {

    // Mage Stats

    // Mage Abilities

    private void Awake()
    {
        abilities.Add(CreateInstance<RPG3_Fireball>());
        abilities.Add(CreateInstance<RPG3_Fireblast>());
        abilities.Add(CreateInstance<RPG3_Evocation>());
        

        resourceType = RPG3_ResourceType.Mana;
        resourcePerSecond = 1;

    }
}

// Projectile Damage
public class RPG3_Fireball : RPG3_Ability
{
    float damage = 25.0f;    
    int cost = 30;
    float range = 40.0f;

    float channelTime = 2.0f;
    float cooldown = 2.0f;    

    public override string GetName() { return "Fireball"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Amulet_1"); }

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

        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().GetCurrentResource() > cost)
        {
            // Check if owner is within distance of the target || RANGED ATTACK, 40 units
            if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < range)
            {
                settings.owner.GetComponent<TPSC_RPG1>().isChanneling = true;
                settings.owner.GetComponent<TPSC_RPG1>().channelledAbility = this;  
                settings.owner.GetComponent<TPSC_RPG1>().channelTimer = channelTime;
                
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

// Instant Cast Damage
public class RPG3_Fireblast : RPG3_Ability
{
    float damage = 25.0f;
    int cost = 40;
    float range = 25.0f;
    float cooldown = 5.0f;   

    public override string GetName() { return "Fireblast"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Stone_2"); }

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
            // Check if owner is within distance of the target || RANGED ATTACK, 25 units
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
        settings.targetEntity.DamageEntity(damage, null);
    }
}

// Recharge Mana
public class RPG3_Evocation : RPG3_Ability
{
    float channelTime = 2.5f;
    float cooldown = 30.0f;
    
    public override string GetName() { return "Evocation"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Amulet_2"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        // Do a thing;
        if (cdTimer <= 0)
        {
            Debug.Log("Evocation Activated");

            settings.owner.GetComponent<TPSC_RPG1>().isChanneling = true;
            settings.owner.GetComponent<TPSC_RPG1>().channelledAbility = this;
            settings.owner.GetComponent<TPSC_RPG1>().channelTimer = channelTime;

            settings.owner.DelayGCD();

            cdTimer = cooldown;
        }

        else
        {
            Debug.Log("On Cooldown: " + cdTimer);
        }
        
    }

    public override void Effect(RPG3_AbilitySettings settings)
    {      
        settings.owner.GetComponent<TPSC_RPG1>().GetEntity().resource = settings.owner.GetComponent<TPSC_RPG1>().GetEntity().maxResource;
    }   
}

