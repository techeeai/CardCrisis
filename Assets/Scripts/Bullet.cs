using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Robot")) return;
        
        var stats = other.GetComponent<EnemyStats>();
        Debug.Log("Has EnemyStats? " + (stats != null));
        if (stats != null)
            stats.TakeDamage(damage);
        Destroy(gameObject);
        
        Debug.Log("Bullet hit: " + other.name + " tag=" + other.tag);
    }
}