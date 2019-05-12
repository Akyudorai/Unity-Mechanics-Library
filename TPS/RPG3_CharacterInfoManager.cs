using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RPG3_CharacterInfoManager : MonoBehaviour {

    public TPSC_RPG1 controller;

    // Canvas
    [SerializeField] Text classText;
    [SerializeField] Text strengthText;
    [SerializeField] Text staminaText;
    [SerializeField] Text agilityText;
    [SerializeField] Text intellectText;
    [SerializeField] Text wisdomText;
    [SerializeField] Text speedText;

    [SerializeField] Text healthText;
    [SerializeField] Text attackPowerText;
    [SerializeField] Text physCritText;
    [SerializeField] Text spellpowerText;
    [SerializeField] Text magiCritText;

    private void Start()
    {
        if (controller != null)
        {
            classText.text = "Class: " + controller.GetClass().className;
            staminaText.text = "Stamina: " + controller.GetClass().stamina;
            strengthText.text = "Strength: " + controller.GetClass().strength;
            agilityText.text = "Agility: " + controller.GetClass().agility;
            intellectText.text = "Intellect: " + controller.GetClass().intellect;
            wisdomText.text = "Wisdom: " + controller.GetClass().wisdom;
            speedText.text = "Movement Speed: " + controller.speedMultiplier * 100 + "%";

            healthText.text = "Health: " + controller.GetEntity().GetCurrentHealth() + " / " + controller.GetEntity().GetMaxHealth();
            attackPowerText.text = "Attack Power: " + controller.GetClass().attackPower;            
            physCritText.text = "Phys Crit: " + controller.GetClass().physicalCritChance * 100.0f + "%";
            spellpowerText.text = "Spell Power: " +controller.GetClass().spellPower;
            magiCritText.text = "Magic Crit: " + controller.GetClass().magicalCritChance * 100 + "%";
        }
    }

    private void Update()
    {
       
    }

}
