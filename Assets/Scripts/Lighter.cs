using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class Lighter : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject Cup;
    [SerializeField] bool IsCup;
    [SerializeField] bool IsLighter;
    [SerializeField] bool IsFlint;
    [SerializeField] GameObject Fire;
    [SerializeField] Light2D globalLight;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float LighterScale;
    private Camera mainCamera;
    private Vector3 offset;
    [SerializeField] private BoxCollider2D LightCollider;
    [SerializeField] private LayerMask collisionMask;

    private bool IsWorking;


    private void OnEnable()
    {
        if (IsLighter)
        {
            mainCamera = Camera.main;

        }
        if (IsFlint)
        {

            globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
            Debug.Log(globalLight);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsCup)
        {
            //There Gonna Be Animation
            Destroy(Cup);
        }

        if (IsFlint)
        {
            if (IsWorking)
                return;
            //play starting sound
            //play anim
            Debug.Log("pressed");
            bool Fired = Random.Range(0f, 1f) <= 0.7f;

            if (Fired)
            {

                Debug.Log(Fired);//true
                Fire.SetActive(true);
                globalLight.intensity = 0.025f;
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
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(eventData.position);
            offset = transform.position - mouseWorldPos;
            offset.z = 0;
            //SigaCollider.enabled = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsLighter)
        {
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(eventData.position) + offset;
            targetPosition.z = transform.position.z;

            // Проверка коллизий перед перемещением
            if (!Physics2D.OverlapCircle(targetPosition, LightCollider.bounds.extents.magnitude * 0.5f, collisionMask))
            {
                transform.position = targetPosition;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsLighter)
        {

        }
    }
}
