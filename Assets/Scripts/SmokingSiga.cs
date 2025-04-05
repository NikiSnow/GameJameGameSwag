using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmokingSiga : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler ,  IEndDragHandler
{
    private GameObject Siga;
    [SerializeField] private GameObject Chapman;
    [SerializeField] private CapsuleCollider2D SigaCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float SigaScale;
    private float pixelsPerUnit;
    private Camera mainCamera;
    private Vector3 offset;

    [SerializeField] private bool IsSiga;
    [SerializeField] private bool IsMouthTrigger;
    [SerializeField] private SmokingSiga MainSiga;
    [SerializeField] private LayerMask collisionMask;
    private bool SigaInDaMouth;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float collisionOffset = 0.1f;

    [SerializeField] private GameObject Zippka;
    private bool Placed;

    bool picked = false;

    private void Awake()
    {
        if (IsSiga)
        {
            SigaScale = transform.localScale.x;
            Debug.Log(SigaScale);
            mainCamera = Camera.main;
            pixelsPerUnit = mainCamera.orthographicSize * 2 / Screen.height;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsMouthTrigger)
        {
            MainSiga.SigaInDaMouth = true;
            Debug.Log("SigaInDaMouth");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsMouthTrigger)
        {
            MainSiga.SigaInDaMouth = false;
            Debug.Log("SigaInDaMouthFALSE");
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
            Placed = true;
        }
        SigaCollider.enabled = true;
    }
}
