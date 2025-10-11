using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 7;

    private Vector2 dir;
    private bool isMoving = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Vector2.right; // 초기 이동 방향
    }

    private void Update()
    {
        if (!isMoving)
        {
            // 멈춘 상태에서만 입력 받기
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) input.y += 1;
            if (Input.GetKey(KeyCode.S)) input.y -= 1;
            if (Input.GetKey(KeyCode.A)) input.x -= 1;
            if (Input.GetKey(KeyCode.D)) input.x += 1;

            if (input != Vector2.zero)
            {
                input.Normalize();
                dir = input;
                isMoving = true; // 입력이 들어오면 다시 미끄러지기 시작
            }
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
        // 벽 레이어 감지
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isMoving = false; // 벽에 닿으면 멈춤
            rb.linearVelocity = Vector2.zero;
        }
    }
}
