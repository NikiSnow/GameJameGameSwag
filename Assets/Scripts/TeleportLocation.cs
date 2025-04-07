using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportLocation : MonoBehaviour
{
    [SerializeField] GameObject UiFBut;
    [SerializeField] Camera MainCum;
    [SerializeField] Transform MovePoint;
    [SerializeField] GameObject Player;

    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    [SerializeField] PolygonCollider2D NextConfiner;

    private bool InTrigger;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UiFBut.SetActive(true);
            InTrigger=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UiFBut.SetActive(false);
            InTrigger = false;
        }
    }
    private void Update()
    {
        if (InTrigger)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Teleport();
            }
        }
    }
    public void Teleport()
    {
        Player.GetComponent<Transform>().position = MovePoint.position;
        VirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = NextConfiner;
    }
}
