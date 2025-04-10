using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SchoolShoting : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator;
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("titri");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ON");
        GameObject hoveredObject = eventData.pointerEnter;

        if (hoveredObject == null) return;

        animator.SetTrigger("KillMe");
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OFF");
        GameObject hoveredObject = eventData.pointerEnter;
        animator.SetTrigger("NotKillMe");
    }
}
