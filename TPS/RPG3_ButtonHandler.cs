using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class RPG3_ButtonHandler : MonoBehaviour {
    
    public RPG3_Ability ability;

    //public UnityAction buttonEvent;

    //public void SetButtonEvent(UnityAction buttonEvent)
    //{
    //    this.buttonEvent = buttonEvent;
    //}

    //public void Invoke()
    //{
    //    buttonEvent.Invoke();
    //}

    public RPG3_Ability GetAbility()
    {
        return ability;
    }
}
