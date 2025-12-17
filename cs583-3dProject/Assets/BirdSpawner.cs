using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 3f;

    [Header("Spawn Paths")]
    public Transform[] spawnStarts;
    public Transform[] spawnEnds;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        if (birdPrefab == null || spawnStarts.Length == 0 || spawnEnds.Length == 0)
            return;

        int index = Random.Range(0, Mathf.Min(spawnStarts.Length, spawnEnds.Length));

        Transform start = spawnStarts[index];
        Transform end = spawnEnds[index];

        GameObject birdObj = Instantiate(birdPrefab, start.position, Quaternion.identity);

        Vector3 direction = (end.position - start.position).normalized;
        BirdEnemy bird = birdObj.GetComponent<BirdEnemy>();

        if (bird != null)
            bird.Init(direction);
    }
}
