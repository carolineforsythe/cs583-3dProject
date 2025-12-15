using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 6f;   // How far up it goes
    public float moveSpeed = 2f;       // Speed of movement
    public float pauseTime = 1f;       // Pause at top/bottom
    public float startDelay = 0f;      // Unique delay for each platform

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.up * moveDistance;

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        // Wait before starting movement (unique per platform)
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Move up
            yield return StartCoroutine(MoveTo(endPos));
            yield return new WaitForSeconds(pauseTime);

            // Move back down
            yield return StartCoroutine(MoveTo(startPos));
            yield return new WaitForSeconds(pauseTime);
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target; // Snap to exact target
    }
}
