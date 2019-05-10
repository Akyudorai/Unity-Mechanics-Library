using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class UI_DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform slotPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(slotPanel, Input.mousePosition))
        {
            Debug.Log("Dropped");
        }
    }
}
