using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RPG3_ClassList
{
    Warrior,
    Rogue,
    Mage
}


public enum RPG3_ResourceType
{
    Rage,
    Mana,
    Energy
}


public class RPG3_Class : ScriptableObject {

    public TPS_Entity entity;

    public List<RPG3_Ability> abilities = new List<RPG3_Ability>();
    
    public RPG3_ResourceType resourceType;
    protected int resourcePerSecond;
    
    public int comboLevel;

    private float resourceTick;

    public void Tick()
    {        
        // Ability Cooldowns
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i].GetCooldown() > 0)
            {
                abilities[i].Cooldown();
            }
        }

        // Resource Management
        if (resourceTick < 1)
        {
            if (resourceType == RPG3_ResourceType.Energy)
                resourceTick += Time.deltaTime * 2;
            else 
                resourceTick += Time.deltaTime;
        }

        if (resourceTick >= 1)
        {
            resourceTick = 0;

            if (entity.resource + resourcePerSecond <= entity.maxResource && entity.resource + resourcePerSecond >= 0)
            {
                entity.AddResource(resourcePerSecond);
            }

            else if (entity.resource + resourcePerSecond > entity.maxResource)
            {
                entity.resource = entity.maxResource;
            }

            else if (entity.resource + resourcePerSecond < 0)
            {
                entity.resource = 0;
            }
            
        }
    }

    public RPG3_Ability GetAbility(string name)
    {
        for (int i = 0; i < abilities.Count; i++)
        {           
            if (abilities[i].GetName() == name)
            {
                return abilities[i];
            }
        }

        return null;
    }
}
