using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float lifeTime = 10f;

    private Vector3 moveDir;

    public void Init(Vector3 direction)
    {
        moveDir = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        if (moveDir.sqrMagnitude > 0.001f)
            transform.forward = moveDir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monkey")) return;

        PlayerController3 player = other.GetComponent<PlayerController3>();
        if (player == null) return;

        // prevent double-hits while respawning / falling
        if (!player.canTakeDamage || player.hasFallen) return;

        // "kill" player: remove a life and respawn using YOUR exact pattern
        player.hasFallen = true;
        PlayerController3.numLivesLeft--;

        if (PlayerController3.numLivesLeft <= 0)
        {
            SceneManager.LoadScene("Level2Lose");
            return;
        }

        // teleport respawn (same as your code)
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // reset velocity in your controller
        // (velocity is private, so we can't directly set it here)
        // workaround: just respawn cleanly; your gravity will settle next frames

        other.transform.position = player.RespawnPoint.position;
        other.transform.rotation = player.RespawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        // reset hasFallen after delay (same as your coroutine behavior)
        player.StartCoroutine(ResetPlayerFallFlag(player));

        Destroy(gameObject);
    }

    private IEnumerator ResetPlayerFallFlag(PlayerController3 player)
    {
        yield return new WaitForSeconds(1f);
        player.hasFallen = false;
    }
}
