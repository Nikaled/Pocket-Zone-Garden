using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteButtonInInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool inContext;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inContext)
        {
            gameObject.SetActive(inContext);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inContext = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inContext = false;
    }
}
