using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 4f;

    [Header("Spawn Paths (Start[i] pairs with End[i])")]
    public Transform[] snakeStarts;
    public Transform[] snakeEnds;

    [Header("Limits")]
    public int maxSnakesAlive = 5;

    private float timer;
    private int aliveCount = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnSnake();
        }
    }

    void SpawnSnake()
    {
        if (snakePrefab == null) return;
        if (snakeStarts == null || snakeEnds == null) return;
        if (snakeStarts.Length == 0 || snakeEnds.Length == 0) return;
        if (aliveCount >= maxSnakesAlive) return;

        int index = Random.Range(0, Mathf.Min(snakeStarts.Length, snakeEnds.Length));

        Transform start = snakeStarts[index];
        Transform end = snakeEnds[index];

        GameObject snakeObj = Instantiate(snakePrefab, start.position, Quaternion.identity);

        SnakeEnemy snake = snakeObj.GetComponent<SnakeEnemy>();
        if (snake != null)
            snake.Init(start.position, end.position);

        aliveCount++;

        // decrement when destroyed
        SnakeAliveTracker tracker = snakeObj.AddComponent<SnakeAliveTracker>();
        tracker.spawner = this;
    }

    // tiny helper component
    private class SnakeAliveTracker : MonoBehaviour
    {
        public SnakeSpawner spawner;
        void OnDestroy()
        {
            if (spawner != null) spawner.aliveCount = Mathf.Max(0, spawner.aliveCount - 1);
        }
    }
}

