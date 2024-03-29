﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum RPG3_AbilityType
{
    Active,
    Passive
}

public enum RPG3_AbilityMode
{
    Targeted,
    Positional,
    Self
}


public class RPG3_AbilitySettings
{
    // Owner of the Ability
    public TPS_Controller owner;

    public GameObject indicator;
    
    
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

        if (target != null)
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
    public abstract RPG3_AbilityMode GetAbilityMode();
    public abstract Sprite GetIcon();
    
    protected float cdTimer;
    
    public bool isTargeting;

    public void Cooldown() {
        cdTimer -= Time.deltaTime;
    }

    public abstract float GetFullCooldown();
    public abstract float GetCooldown();

    public abstract void Activate(RPG3_AbilitySettings settings);
    public abstract void Effect(RPG3_AbilitySettings settings);

}
