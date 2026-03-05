using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float padding = 0.3f;

    private Rigidbody2D rb;
    private Camera cam;
    private float minX, maxX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Start()
    {
        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, -cam.transform.position.z));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, -cam.transform.position.z));

        minX = left.x + padding;
        maxX = right.x - padding;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameplayActive)
            return;

        float move = Input.GetAxis("Horizontal");
        Vector2 pos = rb.position;
        pos.x += move * speed * Time.fixedDeltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        rb.MovePosition(pos);
    }
}