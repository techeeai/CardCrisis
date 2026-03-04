using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RobotMovement : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Yukarı doğru fizik sistemiyle hareket
        rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);
    }
}