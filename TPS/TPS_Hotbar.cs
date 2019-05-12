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
    public Button[] buttons = new Button[10];
    public Image[] icons = new Image[10];

    // Awake
    private void Awake()
    {        
        for (int i = 0; i < 10; i++)
        {
            var test = Resources.Load<Sprite>("Sprites/Empty");
            icons[i].sprite = test;
        }
    }

    private void Update()
    {
        // Update Hotbar
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                if (abilities[i].GetCooldown() > 0)
                {
                    icons[i].fillAmount = (1 - abilities[i].GetCooldown() / abilities[i].GetFullCooldown());
                }

                else if (controller.delay > 0)
                {
                    icons[i].fillAmount = (1 - controller.delay / controller.GCD);
                }
            }
        }
    }

    //public void UpdateIcons()
    //{
    //    //for (int i = 0; i < 10; i++)
    //    //{
    //    //    if (abilities[i] == null)
    //    //    {
    //    //        var test = Resources.Load<Sprite>("Sprites/Empty");
    //    //        icons[i].sprite = test;
    //    //    }

    //    //    else
    //    //    {
    //    //        icons[i].sprite = abilities[i].GetIcon();

    //    //    }
    //    //}

    //}

    // Hotkey System
    public void HotkeyTracker()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {            
            HotkeyDown(1);
            
            //if (abilities[0] != null)
            //{
            //    if (controller.GetDelay() <= 0 && abilities[0].GetCooldown() <= 0)
            //    {
            //        RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
            //        abilities[0].Activate(settings);
            //    }

            //    else
            //    {
            //        Debug.Log("Ability is on cooldown!");
            //    }


            //}

            //else Debug.Log("No Ability in slot 1");

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HotkeyDown(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HotkeyDown(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HotkeyDown(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HotkeyDown(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            HotkeyDown(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            HotkeyDown(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            HotkeyDown(8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            HotkeyDown(9);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            HotkeyDown(0);
        }
        
    }

    public void HotkeyDown(int index)
    {
        switch (index)
        {
            case 1:
                Debug.Log("'1' Pressed");

                if (controller.GetDelay() <= 0)
                {
                    buttons[0].onClick.Invoke();
                }
                
                break;

            case 2:
                Debug.Log("'2' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[1].onClick.Invoke();
                }
                break;

            case 3:
                Debug.Log("'3' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[2].onClick.Invoke();
                }
                break;

            case 4:
                Debug.Log("'4' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[3].onClick.Invoke();
                }
                break;

            case 5:
                Debug.Log("'5' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[4].onClick.Invoke();
                }
                break;

            case 6:
                Debug.Log("'6' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[5].onClick.Invoke();
                }
                break;

            case 7:
                Debug.Log("'7' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[6].onClick.Invoke();
                }
                break;

            case 8:
                Debug.Log("'8' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[7].onClick.Invoke();
                }
                break;

            case 9:
                Debug.Log("'9' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[8].onClick.Invoke();
                }
                break;

            case 0:
                Debug.Log("'0' Pressed");
                if (controller.GetDelay() <= 0)
                {
                    buttons[9].onClick.Invoke();
                }
                break;

            default: break;
        }
    }
}
