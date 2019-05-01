using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class FPS_Button : MonoBehaviour, I_Interactable
{
    public UnityEvent buttenEvent;

    private void Awake()
    {
       // if (buttenEvent != null)
            //buttenEvent = new UnityEvent();
    }

    public FPS_InteractionType GetInteraction()
    {
        return FPS_InteractionType.Button;
    }

    public void Interact()
    {
        buttenEvent.Invoke();
    }
}
