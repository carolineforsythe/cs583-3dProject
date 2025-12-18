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

        // variation so all birds do not sound the same
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.Play();
        }
    }

    void Update()
    {
        // move bird
        transform.position += moveDir * speed * Time.deltaTime;

        // face direction of travel
        if (moveDir.sqrMagnitude > 0.001f)
            transform.forward = moveDir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monkey")) return;

        PlayerController3 player = other.GetComponent<PlayerController3>();
        if (player == null) return;

        if (!player.canTakeDamage || player.hasFallen) return;

        // decrease one life using monkey fall logic
        player.hasFallen = true;
        PlayerController3.numLivesLeft--;

        if (PlayerController3.numLivesLeft <= 0)
        {
            SceneManager.LoadScene("Level3Lose");
            return;
        }

        // respawn player (
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
