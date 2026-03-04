using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float fireRate = 6f; // saniyede 6 mermi

    private float nextTime;

    private void Update()
    {
        if (Time.time < nextTime) return;

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
            nextTime = Time.time + (1f / fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var rb = b.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.up * bulletSpeed;
    }
}