using UnityEngine;

public class BirdPathFly : MonoBehaviour
{
    public Transform pointB;
    public float speed = 10f;
    public float despawnDistance = 0.3f;

    void Update()
    {
        if (pointB == null) return;

        Vector3 next = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
        Vector3 dir = next - transform.position;

        transform.position = next;

        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, pointB.position) <= despawnDistance)
            Destroy(gameObject);
    }
}
