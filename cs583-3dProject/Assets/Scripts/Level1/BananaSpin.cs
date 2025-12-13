using UnityEngine;

public class BananaSpin : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 120f, 0f);
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);

        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}
