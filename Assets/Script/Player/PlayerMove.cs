using System;
using System.IO.Compression;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator anima;

    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7;

    private Vector2 dir;
    private bool isMoving = false;

    [SerializeField] private float rayLength = 1.0625f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Vector2.zero;
        anima = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            anima.SetBool("Walk", false);

            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) input.y += 1;
            if (Input.GetKey(KeyCode.S)) input.y -= 1;
            if (Input.GetKey(KeyCode.A)) input.x -= 1;
            if (Input.GetKey(KeyCode.D)) input.x += 1;
            if (input.x != 0)
            {
                transform.localScale = new Vector3(input.x * -1, 1, 1);
            }
            if (input != Vector2.zero)
            {
                input.Normalize();
                dir = input;
                isMoving = true;
                CheckCollision();
            }
        }
        else
        {
            anima.SetBool("Walk", true);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            CheckCollision();
            rb.linearVelocity = dir * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void CheckCollision()
    {
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            Vector2 dir1 = new Vector2(dir.x, 0);
            Vector2 dir2 = new Vector2(0, dir.y);
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, dir1, rayLength, wallLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, dir2, rayLength, wallLayer);
            RaycastHit2D hit3 = Physics2D.Raycast(transform.position, dir, rayLength, wallLayer);

            if (hit1.collider != null || hit2.collider != null || hit3.collider != null)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayLength, wallLayer);

            if (hit.collider != null)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
        }
    }
}