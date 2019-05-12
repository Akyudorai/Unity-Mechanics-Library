using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG3_Rogue : RPG3_Class {

    // Rogue Stats

    // Rogue Abilities

    private void Awake()
    {
        className = "Rogue";
        stamina = 12;
        strength = 7;
        agility = 20;
        intellect = 2;
        wisdom = 2;

        // Stats
        baseHealth = 500;
        baseResource = 100;

        attackPower = 10 + (strength * 2) + (agility * 1);
        physicalCritChance = (10.0f/100.0f) + (agility/100.0f);

        spellPower = 0 + (intellect * 1);
        magicalCritChance = (0/100.0f) + (wisdom/100.0f);

        // Abilities
        abilities.Add(CreateInstance<RPG3_Shiv>());
        abilities.Add(CreateInstance<RPG3_Eviscerate>());
        abilities.Add(CreateInstance<RPG3_Grapple>());
        abilities.Add(CreateInstance<RPG3_Sprint>());
       
       
        resourceType = RPG3_ResourceType.Energy;
        resourcePerSecond = 10;

    }    
}

public class RPG3_Shiv : RPG3_Ability
{
    float physicalDamageMultiplier = 0.7f;
    int cost = 45;
    float range = 5.0f;
    float cooldown = 0;
    
    public override string GetName() { return "Shiv"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Sword_2"); }

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
        GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat (
            settings.owner.GetComponent<TPS_Entity>().entityName + "'s <" + GetName() + "> deals <" + damage + "> to <" + settings.targetEntity.entityName + ">.",
            Message.MessageType.combatLog
        );
    }
}

public class RPG3_Eviscerate : RPG3_Ability
{
    float physicalDamageMultiplier = 1.0f;
    float baseDamage = 15.0f;

    int cost = 40;
    float range = 5.0f;
    float cooldown = 0;

    public override string GetName() { return "Eviscerate"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Ax_1"); }

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

        else
        {
            Debug.Log("On Cooldown: " + cdTimer);
        }
        

    }

    public override void Effect(RPG3_AbilitySettings settings)
    {
        float damage = settings.owner.GetComponent<TPSC_RPG1>().GetClass().attackPower * physicalDamageMultiplier;
        float totalDamage = damage + (baseDamage * settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel);

        float critRoll = Random.Range(0.0f, 1.0f);
        if (critRoll <= settings.owner.GetComponent<TPSC_RPG1>().GetClass().physicalCritChance)
        {
            totalDamage *= 2;

            GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat("Critical Hit!", Message.MessageType.combatFlag);
        }

        settings.targetEntity.DamageEntity(totalDamage, null);
        settings.owner.GetComponent<TPSC_RPG1>().GetClass().comboLevel = 0;

        GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat(
            settings.owner.GetComponent<TPS_Entity>().entityName + "'s <" + GetName() + "> deals <" + totalDamage + "> to <" + settings.targetEntity.entityName + ">.",
            Message.MessageType.combatLog
        );
    }
}

public class RPG3_Grapple : RPG3_Ability
{    
    float cooldown = 6.0f;

    public override string GetName() { return "Grapple"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Positional; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Flail"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (!isTargeting)
        {
            settings.owner.GetComponent<TPSC_RPG1>().activeTargetIndicator = Instantiate(Resources.Load<GameObject>("AOE_Indicator"));
            settings.owner.GetComponent<TPSC_RPG1>().activeTargetIndicator.transform.localScale = new Vector3(10, 10, 1);
            settings.owner.GetComponent<TPSC_RPG1>().abilityBeingTargeted = this;

            isTargeting = true;
        }

        else
        {
            // Do a thing;
            if (cdTimer <= 0)
            {
                Debug.Log("Grapple Activated");

                
                Effect(settings);

                settings.owner.DelayGCD();
                cdTimer = cooldown;
            }

            else
            {
                Debug.Log("On Cooldown: " + cdTimer);
            }
        }



    }

    public override void Effect(RPG3_AbilitySettings settings)
    {
        // Move to position
        settings.owner.StartCoroutine(settings.owner.MoveTo(settings.targetPosition, 0.25f));
        
    }
}

public class RPG3_Sprint : RPG3_Ability
{
    float cooldown = 6.0f;

    public override string GetName() { return "Sprint"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Self; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Elixir_5"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }

    public override void Activate(RPG3_AbilitySettings settings)
    {
        // Do a thing;
        if (cdTimer <= 0)
        {
            Debug.Log("Sprint Activated");


            Effect(settings);

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
        // Move to position
        settings.owner.GetComponent<TPSC_RPG1>().StartCoroutine(settings.owner.GetComponent<TPSC_RPG1>().SpeedBuff(1.5f, 10.0f));

    }
}



