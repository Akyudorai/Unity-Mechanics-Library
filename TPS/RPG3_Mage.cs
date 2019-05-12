using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG3_Mage : RPG3_Class {

    // Mage Stats

    // Mage Abilities

    private void Awake()
    {
        className = "Mage";
        stamina = 7;
        strength = 2;
        agility = 3;
        intellect = 20;
        wisdom = 12;

        // Stats
        baseHealth = 500;
        baseResource = 200;
        resourcePerSecond = 1 + (0.25f * wisdom);

        attackPower = 5 + (strength * 2) + (agility * 1);
        physicalCritChance = (0 / 100.0f) + (agility / 100.0f);
        
        spellPower = 0 + (intellect * 1);
        magicalCritChance = (10.0f / 100.0f) + (wisdom / 100.0f);

        // Abilities
        abilities.Add(CreateInstance<RPG3_Fireball>());
        abilities.Add(CreateInstance<RPG3_Fireblast>());
        abilities.Add(CreateInstance<RPG3_Evocation>());
        abilities.Add(CreateInstance<RPG3_Firestorm>());
        

        resourceType = RPG3_ResourceType.Mana;
        resourcePerSecond = 1;

    }
}

// Projectile Damage
public class RPG3_Fireball : RPG3_Ability
{
    float spellDamageMultplier = 1.0f;
    float baseDamage = 35.0f;

    int cost = 30;
    float range = 40.0f;

    float channelTime = 2.0f;
    float cooldown = 2.0f;    

    public override string GetName() { return "Fireball"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Amulet_1"); }

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

            if (settings.owner.GetComponent<TPSC_RPG1>().GetEntity().GetCurrentResource() > cost)
            {
                // Check if owner is within distance of the target || RANGED ATTACK, 40 units
                if (Vector3.Distance(settings.owner.transform.position, settings.target.transform.position) < range)
                {
                    settings.owner.GetComponent<TPSC_RPG1>().isChanneling = true;
                    settings.owner.GetComponent<TPSC_RPG1>().channelledAbility = this;
                    settings.owner.GetComponent<TPSC_RPG1>().channelTimer = channelTime;
                    settings.owner.GetComponent<TPSC_RPG1>().channelTarget = settings.target;

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
        float damage = baseDamage + settings.owner.GetComponent<TPSC_RPG1>().GetClass().spellPower * spellDamageMultplier;

        float critRoll = Random.Range(0.0f, 1.0f);
        if (critRoll <= settings.owner.GetComponent<TPSC_RPG1>().GetClass().magicalCritChance)
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

// Instant Cast Damage
public class RPG3_Fireblast : RPG3_Ability
{
    float spellDamageMultplier = 1.0f;
    float baseDamage = 30.0f;

    int cost = 40;
    float range = 25.0f;
    float cooldown = 5.0f;   

    public override string GetName() { return "Fireblast"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Targeted; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Stone_2"); }

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

        else
        {
            Debug.Log("On Cooldown: " + cdTimer);
        }
        

    }

    public override void Effect(RPG3_AbilitySettings settings)
    {
        float damage = baseDamage + settings.owner.GetComponent<TPSC_RPG1>().GetClass().spellPower * spellDamageMultplier;

        float critRoll = Random.Range(0.0f, 1.0f);
        if (critRoll <= settings.owner.GetComponent<TPSC_RPG1>().GetClass().magicalCritChance * 2)
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

// Recharge Mana
public class RPG3_Evocation : RPG3_Ability
{
    float channelTime = 2.5f;
    float cooldown = 30.0f;
    
    public override string GetName() { return "Evocation"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Self; }
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

public class RPG3_Firestorm : RPG3_Ability
{
    float spellDamageMultplier = 0.5f;
    float baseDamage = 30.0f;

    int cost = 40;

    float channelTime = 2.5f;
    float cooldown = 6.0f;

    public override string GetName() { return "Firestorm"; }
    public override RPG3_AbilityType GetAbilityType() { return RPG3_AbilityType.Active; }
    public override RPG3_AbilityMode GetAbilityMode() { return RPG3_AbilityMode.Positional; }
    public override Sprite GetIcon() { return Resources.Load<Sprite>("64X64/Elixir_6"); }

    public override float GetFullCooldown() { return cooldown; }
    public override float GetCooldown() { return cdTimer; }
    
    public override void Activate(RPG3_AbilitySettings settings)
    {
        if (!isTargeting)
        {
            settings.owner.GetComponent<TPSC_RPG1>().activeTargetIndicator = Instantiate(Resources.Load<GameObject>("AOE_Indicator"));
            settings.owner.GetComponent<TPSC_RPG1>().activeTargetIndicator.transform.localScale = new Vector3(40, 40, 1);
            settings.owner.GetComponent<TPSC_RPG1>().abilityBeingTargeted = this;

            isTargeting = true;
        }

        else {
            // Do a thing;
            if (cdTimer <= 0)
            {
                Debug.Log("Firestorm Activated");

                settings.owner.GetComponent<TPSC_RPG1>().isChanneling = true;
                settings.owner.GetComponent<TPSC_RPG1>().channelledAbility = this;
                settings.owner.GetComponent<TPSC_RPG1>().channelTimer = channelTime;

                settings.owner.GetComponent<TPSC_RPG1>().GetEntity().SubtractResource(cost);
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
        float damage = baseDamage + settings.owner.GetComponent<TPSC_RPG1>().GetClass().spellPower * spellDamageMultplier;

        float critRoll = Random.Range(0.0f, 1.0f);
        if (critRoll <= settings.owner.GetComponent<TPSC_RPG1>().GetClass().magicalCritChance)
        {
            GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat("Critical Hit!", Message.MessageType.combatFlag);
            damage *= 2;
        }

        // Explode, deal damage        
        Collider[] objectsInRange = Physics.OverlapSphere(settings.targetPosition, 5);
        foreach (Collider c in objectsInRange)
        {
            // If there's a healthscript attached, deal damage to it
            if (c.GetComponent<TPS_Entity>() != null)
                if (c.GetComponent<TPS_Entity>() != settings.owner.GetComponent<TPSC_RPG1>().GetEntity())
                {
                    c.GetComponent<TPS_Entity>().DamageEntity(damage, null);

                    GameObject.Find("_GM").GetComponent<ChatManager>().SendMessageToChat(
                        settings.owner.GetComponent<TPS_Entity>().entityName + "'s <" + GetName() + "> deals <" + damage + "> to <" + c.GetComponent<TPS_Entity>().entityName + ">.",
                        Message.MessageType.combatLog
                    );
                }
                    
            


        }
    }    
}
