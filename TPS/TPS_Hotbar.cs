using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TPS_Hotbar : MonoBehaviour {

    // Owner 
    public TPSC_RPG1 controller;

    // Hotbar Canvas
    public Canvas canvas;

    public RPG3_Ability[] abilities = new RPG3_Ability[10];
    public Image[] icons = new Image[10];

    // Awake
    private void Awake()
    {        
        for (int i = 0; i < 10; i++)
        {
            if (abilities[i] == null)
            {
                var test = Resources.Load<Sprite>("Sprites/Empty");
                icons[i].sprite = test;
            }
        }
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < 10; i++)
        {
            if (abilities[i] == null)
            {
                var test = Resources.Load<Sprite>("Sprites/Empty");
                icons[i].sprite = test;
            }

            else
            {
                icons[i].sprite = abilities[i].GetIcon();
            }
        }
    }

    // Hotkey System
    public void HotkeyTracker()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log("'1' Pressed");
            
            if (abilities[0] != null)
            {
                if (controller.GetDelay() <= 0)
                {
                    if (controller.GetCurrentTarget() != null)
                    {
                        RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
                        abilities[0].Activate(settings);                        
                    }

                    else
                    {
                        Debug.Log("No Target Selected");
                    }
                }

                else
                {
                    Debug.Log("Ability is on cooldown!");
                }
                
                
            }

            else Debug.Log("No Ability in slot 1");

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Debug.Log("'2' Pressed");

            if (abilities[1] != null)
            {
                if (controller.GetDelay() <= 0)
                {
                    if (controller.GetCurrentTarget() != null)
                    {
                        RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
                        abilities[1].Activate(settings);
                        controller.DelayGCD();
                    }

                    else
                    {
                        Debug.Log("No Target Selected");
                    }
                }

                else
                {
                    Debug.Log("Ability is on cooldown!");
                }

            }

            else Debug.Log("No Ability in slot 2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("'3' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("'4' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("'5' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("'6' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("'7' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("'8' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("'9' Pressed");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("'0' Pressed");
        }
        
    }

    public void HotkeyDown(int index)
    {
        switch (index)
        {
            case 1:
                Debug.Log("'1' Pressed");

                if (abilities[0] != null)
                {
                    if (controller.GetCurrentTarget() != null)
                    {
                        RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
                        abilities[0].Activate(settings);
                    }

                    else
                    {
                        Debug.Log("No Target Selected");
                    }

                }

                else Debug.Log("No Ability in slot 1");

                break;

            case 2:

                if (abilities[1] != null)
                {
                    if (controller.GetCurrentTarget() != null)
                    {
                        RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
                        abilities[1].Activate(settings);
                    }

                    else
                    {
                        Debug.Log("No Target Selected");
                    }

                }

                else Debug.Log("No Ability in slot 2");

                break;

            case 3:
                Debug.Log("'3' Pressed");
                break;

            case 4:
                Debug.Log("'4' Pressed");
                break;

            case 5:
                Debug.Log("'5' Pressed");
                break;

            case 6:
                Debug.Log("'6' Pressed");
                break;

            case 7:
                Debug.Log("'7' Pressed");
                break;

            case 8:
                Debug.Log("'8' Pressed");
                break;

            case 9:
                Debug.Log("'9' Pressed");
                break;

            case 0:
                Debug.Log("'0' Pressed");
                break;

            default: break;
        }
    }
}
