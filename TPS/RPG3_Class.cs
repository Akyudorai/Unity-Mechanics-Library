using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RPG3_Class : ScriptableObject {

    public List<RPG3_Ability> abilities = new List<RPG3_Ability>();
    
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
