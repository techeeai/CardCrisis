using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.4f;

    private float fireTimer;

    private void Update()
    {
        RotateToMouse();   // Şimdilik dursun

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void RotateToMouse()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouse - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    void Fire()
    {
        if (bulletPrefab == null) return;

        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}