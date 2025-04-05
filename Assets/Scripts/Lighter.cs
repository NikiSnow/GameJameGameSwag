using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lighter : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject Cup;
    [SerializeField] bool IsCup;
    [SerializeField] bool IsLighter;
    [SerializeField] bool IsFlint;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsCup)
        {
            //There Gonna Be Animation
            Destroy(Cup);
        }

        if (IsFlint)
        {
            //play starting sound
            //play anim
            bool Fired = Random.Range(0f, 1f) <= 0.7f;

            if (Fired)
            {
                Debug.Log(Fired);
            }
            else
            {
                Debug.Log(Fired);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsLighter)
        {

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsLighter)
        {

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsLighter)
        {

        }
    }
}
