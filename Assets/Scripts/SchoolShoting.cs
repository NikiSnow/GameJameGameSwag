using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SchoolShoting : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer Me1;
    [SerializeField] private SpriteRenderer Me2;

    [SerializeField] private SpriteRenderer Fa1;
    [SerializeField] private SpriteRenderer Fa2;
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("titri");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ON");
        GameObject hoveredObject = eventData.pointerEnter;

        if (hoveredObject == null) return;

        if (hoveredObject.CompareTag("ShootMe"))
        {
            if (Me1 != null) Me1.color = new Color(1, 1, 1, 0.3f);
            if (Me2 != null) Me2.color = new Color(1, 1, 1, 0.3f);
        }
        else if (hoveredObject.CompareTag("ShootFather"))
        {
            if (Fa1 != null) Fa1.color = new Color(1, 1, 1, 0.3f);
            if (Fa2 != null) Fa2.color = new Color(1, 1, 1, 0.3f);
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OFF");
        GameObject hoveredObject = eventData.pointerEnter;
        if (hoveredObject.CompareTag("ShootMe"))
        {
            Me1.color = new Color(1, 1, 1, 0.0f);
            Me2.color = new Color(1, 1, 1, 0.0f);
        }
        else if (hoveredObject.CompareTag("ShootFather"))
        {
            Fa1.color = new Color(1, 1, 1, 0.0f);
            Fa2.color = new Color(1, 1, 1, 0.0f);
        }
    }
}
