using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform RespawnPoint;
    private bool canTakeDamage = true;
    private bool hasFallen = false; // prevent multiple life decrements
    public float moveSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public Transform cameraTransform;
    public Animator animator;
    public int maxHealth = 3;
    public static int numLivesLeft = 3;

    // jump sound
    public AudioSource jumpAudioSource;
    public AudioClip jumpSound;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int currentHealth;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        numLivesLeft = 3;

        // setup jump audio source if not assigned
        if (jumpAudioSource == null)
        {
            jumpAudioSource = gameObject.AddComponent<AudioSource>();
            jumpAudioSource.playOnAwake = false;
            jumpAudioSource.spatialBlend = 0f; // 2D sound
        }
    }

    System.Collections.IEnumerator ResetFallFlagAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        hasFallen = false;
        print("Fall flag reset");
    }

    void Update()
    {
        // check ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // check input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // move camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * vertical + right * horizontal).normalized;
        if (move.magnitude >= 0.1f)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
            // rotate monkey to face movement direction
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null)
            {
                animator.SetBool("jump1", true);
            }

            // play jump sound with SFX volume
            PlayJumpSound();
        }

        // gravity 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // animations
        if (animator != null)
        {
            bool movingForward = vertical > 0.1f;
            bool movingBackward = vertical < -0.1f;
            animator.SetBool("run", movingForward);
            animator.SetBool("Back run", movingBackward);
            if (isGrounded && velocity.y <= 0f)
            {
                animator.SetBool("jump1", false);
            }
        }

        // fall detection and respawn
        if (transform.position.y < -5f && !hasFallen)
        {
            hasFallen = true;
            numLivesLeft--;
            print("Lives remaining: " + numLivesLeft);
            if (numLivesLeft <= 0)
            {
                SceneManager.LoadScene("Level1Lose");
            }
            else
            {
                // respawn
                print("Attempting respawn to: " + RespawnPoint.position);
                velocity = Vector3.zero;
                controller.enabled = false;
                transform.position = RespawnPoint.position;
                transform.rotation = RespawnPoint.rotation;
                controller.enabled = true;
                print("After respawn, position is: " + transform.position);
                // reset flag
                StartCoroutine(ResetFallFlagAfterDelay());
            }
        }
    }

    void PlayJumpSound()
    {
        if (jumpSound != null && jumpAudioSource != null)
        {
            // get SFX volume from SoundVolumeSFX
            float sfxVolume = SoundVolumeSFX.Instance != null ? SoundVolumeSFX.Instance.GetSFXVolume() : 1f;
            jumpAudioSource.volume = sfxVolume;
            jumpAudioSource.PlayOneShot(jumpSound);
        }
    }
}
