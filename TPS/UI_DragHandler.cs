using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Vector3 startPos;

    [SerializeField] UIRaycaster raycaster;

    private void Awake()
    {
        raycaster = GetComponentInParent<UIRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.localPosition;
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)        
    {
        
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (raycaster.Raycast()[0].gameObject.GetComponent<Button>() != null)
        {
            Debug.Log(raycaster.Raycast()[0]);

            Button button = raycaster.Raycast()[0].gameObject.GetComponent<Button>();
            button.GetComponent<Image>().sprite  = GetComponent<Image>().sprite;
            
            button.onClick = GetComponent<Button>().onClick;
        }

        transform.localPosition = startPos;
        GetComponent<Image>().raycastTarget = true;
    }
}
