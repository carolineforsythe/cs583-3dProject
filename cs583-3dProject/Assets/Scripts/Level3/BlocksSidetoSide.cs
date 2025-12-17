using UnityEngine;

public class BlocksSidetoSide : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // direction to move
    public float distance = 3f;                    // how far it moves
    public float speed = 2f;                       // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + moveDirection.normalized * offset;
    }
}
