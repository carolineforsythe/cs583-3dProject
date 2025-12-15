using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 45f; // Degrees per second

    private void Update()
    {
        // Rotate the platform around its Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When player lands on the platform, make them a child of it
        if (other.CompareTag("Monkey"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When player leaves the platform, unparent them
        if (other.CompareTag("Monkey"))
        {
            other.transform.SetParent(null);
        }
    }
}

