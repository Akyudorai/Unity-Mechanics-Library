using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPG3_Warrior : RPG3_Class {

    // Warrior Stats

    // Warrior Abilities

    private void Awake()
    {
        className = "Warrior";
        stamina = 20;
        strength = 15;
        agility = 5;
        intellect = 2;
        wisdom = 2;

        // Stats
        baseHealth = 500;
        baseResource = 100;

        attackPower = 10 + (strength * 2) + (agility * 1);
        physicalCritChance = (10.0f / 100.0f) + (agility / 100.0f);

        spellPower = 0 + (intellect * 1);
        magicalCritChance = (0 / 100.0f) + (wisdom / 100.0f);

        // Abilities
        abilities.Add(CreateInstance<RPG3_Rend>());
        abilities.Add(CreateInstance<RPG3_Charge>());

        resourceType = RPG3_ResourceType.Rage;        
        resourcePerSecond = -1;

    }   
}

// Deal Damage
public class RPG3_Rend : RPG3_Ability
{
    float physicalDamageMultiplier = 1.25f;
    float cooldown = 0;

    public override string GetName() { return "Rend"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Ax_2"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (cdTimer <= 0)
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

        else
        {
            Debug.Log("On Cooldown: " + cdTimer);
        }
        
        
    }

    public override void Effect(RPG3_AbilitySettings settings)
    {
        float damage = settings.owner.GetComponent<TPSC_RPG1>().GetClass().attackPower * physicalDamageMultiplier;

        float critRoll = Random.Range(0.0f, 1.0f);
        if (critRoll <= settings.owner.GetComponent<TPSC_RPG1>().GetClass().physicalCritChance)
        {
            damage *= 2;
            GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat("Critical Hit!", Message.MessageType.combatFlag);
        }

        settings.targetEntity.DamageEntity(damage, null);

        GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat(
            settings.owner.GetComponent<TPS_Entity>().entityName + "'s <" + GetName() + "> deals <" + damage + "> to <" + settings.targetEntity.entityName + ">.",
            Message.MessageType.combatLog
        );
    }

   
}


// Move to target entity/position
public class RPG3_Charge : RPG3_Ability
{
    float cooldown = 5.0f;
   
    public override string GetName() { return "Charge"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Helmet_4"); }
    

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (cdTimer <= 0)
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

        else
        {
            Debug.Log("On Cooldown: " + cdTimer);
        }


    }

    public override void Effect(RPG3_AbilitySettings settings)
    {        
        settings.owner.disableControls = true;
        settings.owner.StartCoroutine(settings.owner.MoveToEntity(settings.targetEntity, 0.25f));
        

        if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().resource + 40 <= 100)
            settings.owner.GetComponent<TPSC_RPG1>().GetEntity().AddResource(40);
        else
            settings.owner.GetComponent<TPSC_RPG1>().GetEntity().resource = 100;
    }    
}


