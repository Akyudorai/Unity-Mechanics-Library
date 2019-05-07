using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycaster : MonoBehaviour {

    GraphicRaycaster raycaster;    
    [SerializeField] EventSystem eventSystem;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    public List<RaycastResult> Raycast()
    {
        List<RaycastResult> results = new List<RaycastResult>();

        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        raycaster.Raycast(eventData, results);
  
        return results;
    }

}
