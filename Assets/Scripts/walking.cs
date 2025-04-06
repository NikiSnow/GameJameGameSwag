using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wlking : MonoBehaviour
{
    public float speed;
    private float moveInput;

    private Rigidbody2D rb;
    public bool AbleMove=true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }
}
