using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    private Animator anima;

    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7;

    [SerializeField] private float maxDashSpeed = 15;
    private float dashSpeed = 1;
    private bool dashing = false;

    private Vector2 dir;
    private bool isMoving = false;

    [SerializeField] private float rayLength = 0.53125f;

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

            if (Input.GetKey(KeyCode.LeftShift))
            {
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
                    dashing = true;
                    dashSpeed = maxDashSpeed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            CheckCollision();

            if (dashing)
            {
                rb.linearVelocity = dir * dashSpeed;
                dashSpeed -= 0.5f;
                if (dashSpeed <= speed)
                {
                    dashing = false;
                }
            }
            else
            {
                rb.linearVelocity = dir * speed;
            }
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
            Vector2 dirX = new Vector2(dir.x, 0);
            Vector2 dirY = new Vector2(0, dir.y);
            RaycastHit2D hitX = Physics2D.Raycast(transform.position, dirX, rayLength, wallLayer);
            RaycastHit2D hitY = Physics2D.Raycast(transform.position, dirY, rayLength, wallLayer);

            if (hitX.collider != null || hitY.collider != null)
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