using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmokingSiga : MonoBehaviour, IPointerClickHandler
{
    private GameObject Siga;
    [SerializeField] private GameObject Chapman;
    bool picked= false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!picked)
        {
            Debug.Log("siga pick me!");
            Destroy(Chapman);
        }

    }

}
