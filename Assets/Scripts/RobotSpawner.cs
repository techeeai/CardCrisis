using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject RobotPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 1f; // kaç saniyede bir spawn
    public float spawnY = -5f;       // alt tarafta doğsun
    public float minX = -2.5f;       // sol sınır
    public float maxX = 2.5f;        // sağ sınır

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRobot();
            timer = 0f;
        }
    }

    void SpawnRobot()
    {
        if (RobotPrefab == null)
        {
            return;
        }

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        GameObject z = Instantiate(RobotPrefab, pos, Quaternion.identity);

        var move = z.GetComponent<RobotMovement>();
    }
}
