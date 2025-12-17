using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float turnSpeed = 10f;
    public float reachDistance = 0.25f;
    public float maxLifeTime = 20f; // safety cleanup

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 targetPos;
    private bool initialized = false;

    public void Init(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;

        // spawn exactly at start
        transform.position = startPos;

        // keep target on same y as snake so it stays "ground level"
        targetPos = endPos;
        targetPos.y = transform.position.y;

        initialized = true;
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        if (!initialized) return;

        Vector3 toTarget = targetPos - transform.position;
        toTarget.y = 0f;

        if (toTarget.magnitude <= reachDistance)
        {
            Destroy(gameObject); // reached end, remove snake
            return;
        }

        Vector3 moveDir = toTarget.normalized;

        // smooth rotate toward travel direction
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
        }

        // move
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monkey")) return;

        PlayerController3 player = other.GetComponent<PlayerController3>();
        if (player == null) return;

        if (!player.canTakeDamage || player.hasFallen) return;

        player.hasFallen = true;
        PlayerController3.numLivesLeft--;

        if (PlayerController3.numLivesLeft <= 0)
        {
            SceneManager.LoadScene("Level3Lose");
            return;
        }

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        other.transform.position = player.RespawnPoint.position;
        other.transform.rotation = player.RespawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        player.StartCoroutine(ResetPlayerFallFlag(player));
    }

    private IEnumerator ResetPlayerFallFlag(PlayerController3 player)
    {
        yield return new WaitForSeconds(1f);
        player.hasFallen = false;
    }
}
