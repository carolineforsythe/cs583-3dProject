using UnityEngine;

public class SnakeSpawnerAreas : MonoBehaviour
{
    [System.Serializable]
    public class SnakeArea
    {
        public string areaName;
        public bool enabled = true;
        public Transform[] starts;
        public Transform[] ends;
    }

    public GameObject snakePrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 4f;

    [Header("Areas")]
    public SnakeArea[] areas; // allows for multiple snakes to spawn and end in different locations

    [Header("Limits")]
    public int maxSnakesAlive = 6; // 6 snakes can be on screen at once

    private float timer;
    private int aliveCount = 0;

    void Update()
    {
        // spawn according to time interval 
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFromRandomArea();
        }
    }

    void SpawnFromRandomArea()
    {
        if (snakePrefab == null) return;
        if (aliveCount >= maxSnakesAlive) return;
        if (areas == null || areas.Length == 0) return;

        // pick an enabled area that has valid paths
        int safety = 50;
        while (safety-- > 0)
        {
            var area = areas[Random.Range(0, areas.Length)];
            if (!area.enabled) continue;
            if (area.starts == null || area.ends == null) continue;
            if (area.starts.Length == 0 || area.ends.Length == 0) continue;

            int idx = Random.Range(0, Mathf.Min(area.starts.Length, area.ends.Length));
            Transform start = area.starts[idx];
            Transform end = area.ends[idx];
            if (start == null || end == null) continue;

            GameObject snakeObj = Instantiate(snakePrefab, start.position, Quaternion.identity);
            SnakeEnemy snake = snakeObj.GetComponent<SnakeEnemy>();
            if (snake != null) snake.Init(start.position, end.position);

            aliveCount++;

            var tracker = snakeObj.AddComponent<SnakeAliveTracker>();
            tracker.spawner = this;
            return;
        }
    }

    private class SnakeAliveTracker : MonoBehaviour
    {
        public SnakeSpawnerAreas spawner;
        void OnDestroy()
        {
            if (spawner != null) spawner.aliveCount = Mathf.Max(0, spawner.aliveCount - 1);
        }
    }
}

