using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator anima;

    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7;

    private Vector2 dir;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Vector2.zero;
        anima = GetComponent<Animator>();
    }

    private void Update()
    {
        if (rb.linearVelocity == Vector2.zero)
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
            rb.linearVelocity = dir * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌");
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isMoving = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}