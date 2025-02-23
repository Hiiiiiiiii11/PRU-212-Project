using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [Header("Di chuyển")]
    public float walkSpeed = 3f;      // Tốc độ di chuyển của enemy
    public float leftDistance = 3f;   // Khoảng cách tối đa di chuyển sang trái
    public float rightDistance = 3f;  // Khoảng cách tối đa di chuyển sang phải

    [Header("Ground Check")]
    public Transform groundCheck;     // Vị trí kiểm tra "on ground"
    public float groundCheckRadius = 0.2f; // Bán kính kiểm tra
    public LayerMask groundLayer;     // Layer của ground

    private float leftLimit;          // Giá trị x giới hạn bên trái
    private float rightLimit;         // Giá trị x giới hạn bên phải

    private Rigidbody2D rb;
    private bool movingRight = true;  // Hướng di chuyển ban đầu (Right)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Xác định giới hạn di chuyển dựa vào vị trí ban đầu của enemy
        leftLimit = transform.position.x - leftDistance;
        rightLimit = transform.position.x + rightDistance;
    }

    void FixedUpdate()
    {
        // Kiểm tra enemy có đang đứng trên ground không
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Nếu không trên ground, không áp dụng di chuyển patrol
        if (!isGrounded)
        {
            // Có thể cho enemy rơi tự nhiên nếu bay ra khỏi ground
            return;
        }

        // Lấy vị trí hiện tại của enemy
        Vector2 currentPos = transform.position;

        // Nếu đang di chuyển sang phải
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
            // Nếu vượt quá giới hạn bên phải, đổi hướng
            if (currentPos.x >= rightLimit)
            {
                movingRight = false;
                Flip();
            }
        }
        // Nếu đang di chuyển sang trái
        else
        {
            rb.linearVelocity = new Vector2(-walkSpeed, rb.linearVelocity.y);
            // Nếu vượt quá giới hạn bên trái, đổi hướng
            if (currentPos.x <= leftLimit)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    // Hàm lật sprite của enemy theo trục X
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
