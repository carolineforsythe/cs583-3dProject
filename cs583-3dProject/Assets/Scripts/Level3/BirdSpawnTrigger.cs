using UnityEngine;

public class BirdIntervalSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform spawnPoint;
    public Transform endPoint;

    public float spawnInterval = 3f;
    public bool startOnPlay = true;

    private float timer;
    private bool spawning;

    void Start()
    {
        spawning = startOnPlay;
        timer = spawnInterval;
    }

    void Update()
    {
        if (!spawning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnBird();
            timer = spawnInterval;
        }
    }

    public void StartSpawning()
    {
        spawning = true;
        timer = spawnInterval;
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    void SpawnBird()
    {
        GameObject bird = Instantiate(birdPrefab, spawnPoint.position, spawnPoint.rotation);

        BirdPathFly fly = bird.GetComponent<BirdPathFly>();
        if (fly != null)
            fly.pointB = endPoint;
    }
}

