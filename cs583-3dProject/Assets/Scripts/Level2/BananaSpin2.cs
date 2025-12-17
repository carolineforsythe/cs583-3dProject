using UnityEngine;

public class BananaSpin2 : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 120f, 0f);
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    private Vector3 bobOffset;

    void Start()
    {
        bobOffset = transform.localPosition;

        // Check if a moving platform is directly below at start
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            if (hit.collider.CompareTag("Moving"))
            {
                transform.SetParent(hit.collider.transform);
                bobOffset = transform.localPosition;
            }
        }
    }

    void Update()
    {
        // Rotate banana
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);

        // Bobbing motion
        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = bobOffset + new Vector3(0f, yOffset, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(collision.transform);
            bobOffset = transform.localPosition;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(null);
        }
    }
}
