using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wlking : MonoBehaviour
{
    public float speed;
    private float moveInput;

    private Rigidbody2D rb;
    public bool AbleMove = true;
    ItemPickupSystem itemPickupSystem;
    [SerializeField] private FallRecoverySystem FallRecoverySystem;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject DeadScreen;
    [SerializeField] private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemPickupSystem = this.gameObject.GetComponent<ItemPickupSystem>();
    }

    private void Update()
    {
        if (!AbleMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (rb.velocityX != 0)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    public void StartedFalling()
    {
        animator.SetTrigger("StartedFalling");
    }
    public void StartedStanding()
    {
        animator.SetTrigger("StartedStanding");
    }
    public void LockMovement()
    {
        AbleMove = false;
        animator.SetBool("Walking", false);
    }
    public void UNLockMovement()
    {
        AbleMove = true;
    }

    public void ShowDeadScreen()
    {
        DeadScreen.SetActive(true);
        AbleMove = false;
    }

    public void GoBackMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Dead()
    {
        if (itemPickupSystem.RespawnOnLastCheckP())
        {
            this.gameObject.transform.position = itemPickupSystem.CheckPointPosition.position;
            Pointer.SetActive(false);
            Pointer.SetActive(true);
            AbleMove = true;
            DeadScreen.SetActive(false);
            FallRecoverySystem.StandUp();
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}
