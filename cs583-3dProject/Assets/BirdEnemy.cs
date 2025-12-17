using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float lifeTime = 10f;

    private Vector3 moveDir;

    [Header("Audio")]
    public AudioSource audioSource;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    // Called by spawner
    public void Init(Vector3 direction)
    {
        moveDir = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Start()
    {
        // Get AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Slight variation so multiple birds don't sound identical
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.Play();
        }
    }

    void Update()
    {
        // Move bird
        transform.position += moveDir * speed * Time.deltaTime;

        // Face direction of travel
        if (moveDir.sqrMagnitude > 0.001f)
            transform.forward = moveDir;
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
            SceneManager.LoadScene("Level2Lose");
            return;
        }

        // Respawn player (same logic you already use)
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        other.transform.position = player.RespawnPoint.position;
        other.transform.rotation = player.RespawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        player.StartCoroutine(ResetPlayerFallFlag(player));

        Destroy(gameObject);
    }

    IEnumerator ResetPlayerFallFlag(PlayerController3 player)
    {
        yield return new WaitForSeconds(1f);
        player.hasFallen = false;
    }
}
