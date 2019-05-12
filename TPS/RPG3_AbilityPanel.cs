using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class RPG3_AbilityPanel : MonoBehaviour {

	[SerializeField] GameObject buttonParent;
    [SerializeField] GameObject buttonObj;
    
    public void Initialize(TPSC_RPG1 controller, RPG3_Ability ability)
    {
        GameObject o = Instantiate(buttonObj, buttonParent.transform);
        
        UnityAction action = delegate {
            RPG3_AbilitySettings settings = RPG3_AbilitySettings.Initialize(controller, controller.GetCurrentTarget());
            ability.Activate(settings);
        };
                
        o.GetComponentInChildren<Button>().onClick.AddListener(action);
        o.GetComponentInChildren<RPG3_ButtonHandler>().ability = ability;
        
        o.GetComponentsInChildren<Image>()[1].sprite = ability.GetIcon();
        
    }    
}
