using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class SmokingSiga : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler ,  IEndDragHandler
{
    private GameObject Siga;
    [SerializeField] private GameObject Chapman;
    [SerializeField] private CapsuleCollider2D SigaCollider;
    [SerializeField] private Rigidbody2D rb;


    private Camera mainCamera;
    private Vector3 offset;

    [SerializeField] private bool IsSiga;
    [SerializeField] private bool IsMouthTrigger;
    [SerializeField] private bool IsFireTrigger; 
    [SerializeField] private SmokingSiga MainSiga;
    [SerializeField] private LayerMask collisionMask;
    private bool SigaInDaMouth;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float collisionOffset = 0.1f;

    [SerializeField] private GameObject Zippka;
    [SerializeField] private GameObject FireZone;
    [SerializeField] private GameObject FireLight;
    private bool Placed;

    bool picked = false;

    private void OnEnable()
    {
        //Debug.Log("SigaScreenFromSigaStarted");
        if (IsSiga)
        {
            mainCamera = Camera.main;
            Debug.Log(mainCamera);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsMouthTrigger)
        {
            if (collision.CompareTag("Siga"))
            {
                MainSiga.SigaInDaMouth = true;
                Debug.Log("SigaInDaMouth");
            }

        }
        if (IsFireTrigger )
        {

            // Проверка по тегу (например, "Player")
            if (collision.CompareTag("Lighter"))
            {
                Debug.Log("Fired");
                //FireZone.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
                FireLight.SetActive(true);
                GameObject.FindWithTag("BalancePointer").GetComponent<Pointer>().GoBackAfterSmoking();
                Light2D globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
                globalLight.intensity = 1f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsMouthTrigger)
        {
            if (collision.CompareTag("Siga"))
            {
                MainSiga.SigaInDaMouth = false;
                Debug.Log("SigaInDaMouthFALSE");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!picked)
        {
            Debug.Log("siga pick me!");
            Destroy(Chapman);
            transform.rotation = Quaternion.identity;
            picked = true;

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!picked)
            return;
        if (Placed)
            return;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        offset = transform.position - mouseWorldPos;
        offset.z = 0;
        //SigaCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!picked)
            return;
        if (Placed)
            return;

        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(eventData.position) + offset;
        targetPosition.z = transform.position.z;

        // Проверка коллизий перед перемещением
        if (!Physics2D.OverlapCircle(targetPosition, SigaCollider.bounds.extents.magnitude * 0.5f, collisionMask))
        {
            transform.position = targetPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!picked)
            return;
        if (SigaInDaMouth)
        {
            Zippka.SetActive(true);
            FireZone.SetActive(true);
            Placed = true;
        }
        SigaCollider.enabled = true;
    }
}
