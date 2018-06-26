using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick1 : MonoBehaviour,IDragHandler,IPointerUpHandler
{
    private float Radius = 175f;


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        
    }
}
