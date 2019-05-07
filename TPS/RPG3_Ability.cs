using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPG3_AbilityType
{
    Active,
    Passive
}

//public enum RPG3_AbilityMode
//{
//    Targeted,
//    Positional,
//    Self
//}


public class RPG3_AbilitySettings
{
    // Owner of the Ability
    public TPS_Controller owner;
    
    // #########################################################
    // # ----------------- TARGETED SYSTEM ------------------- #
    // #########################################################
    
    public TPS_Controller target;
    public TPS_Entity targetEntity;

    public static RPG3_AbilitySettings Initialize(TPS_Controller owner, TPS_Controller target)
    {
        RPG3_AbilitySettings result = new RPG3_AbilitySettings();
        result.owner = owner;
        result.target = target;
        result.targetEntity = target.GetComponent<TPS_Entity>();
        
        return result;
    }

    // #########################################################
    // # ---------------- POSITIONAL SYSTEM ------------------ #
    // #########################################################

    public Vector3 targetPosition;

    public static RPG3_AbilitySettings Initialize(TPS_Controller owner, Vector3 targetPosition)
    {
        RPG3_AbilitySettings result = new RPG3_AbilitySettings();
        result.owner = owner;
        result.targetPosition = targetPosition;

        return result;
    }

    // #########################################################
    // # -------------------- SELF SYSTEM -------------------- #
    // #########################################################



    // #########################################################
    // # ----------------------- EOC ------------------------- #
    // #########################################################
}


public abstract class RPG3_Ability : ScriptableObject {

    public abstract string GetName();
    public abstract RPG3_AbilityType GetAbilityType();
    public abstract Sprite GetIcon();


    public abstract void Activate(RPG3_AbilitySettings settings);

}
