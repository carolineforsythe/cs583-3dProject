using UnityEngine;
using System.Collections;

public class PopUpBlock : MonoBehaviour
{
    public float popDistance = 7f;   // How far the block moves out (relative to Z)
    public float popSpeed = 4f;      // Speed of movement
    public float delay = 0f;         // Delay before first pop
    public float pauseTime = 5f;     // Time to wait before toggling back

    private Vector3 hiddenPos;
    private Vector3 shownPos;
    private bool isShown = false;

    void Start()
    {
        // Save the starting position
        hiddenPos = transform.position;

        // Target position is 4 units back along Z
        shownPos = new Vector3(hiddenPos.x, hiddenPos.y, hiddenPos.z - popDistance);

        // Start hidden
        transform.position = hiddenPos;

        // Begin coroutine
        StartCoroutine(PopLoop());
    }

    IEnumerator PopLoop()
    {
        yield return new WaitForSeconds(delay);

        while (true) // infinite loop
        {
            Vector3 target = isShown ? hiddenPos : shownPos;

            // Move toward target
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, popSpeed * Time.deltaTime);
                yield return null;
            }

            // Snap to exact target
            transform.position = target;

            // Flip state
            isShown = !isShown;

            // Pause before switching again
            yield return new WaitForSeconds(pauseTime);
        }
    }
}


