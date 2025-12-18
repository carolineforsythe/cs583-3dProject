using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3 : MonoBehaviour
{
    [Header("Respawn")]
    public Transform RespawnPoint;

    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Animation")]
    public Animator animator;

    [Header("Health & Lives")]
    public int maxHealth = 3;
    public static int numLivesLeft = 3;
    public bool canTakeDamage = true; 
    public bool hasFallen = false;    

    [Header("Fall Damage")]
    public float fatalFallDistance = 2f;

    [Header("Sound")]
    public AudioSource jumpAudioSource;
    public AudioClip jumpSound;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int currentHealth;
    private float fallStartY;
    private bool wasGroundedLastFrame;

    // platform support
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        numLivesLeft = 3;

        wasGroundedLastFrame = controller.isGrounded;
        fallStartY = transform.position.y;

        if (jumpAudioSource == null)
        {
            jumpAudioSource = gameObject.AddComponent<AudioSource>();
            jumpAudioSource.playOnAwake = false;
            jumpAudioSource.spatialBlend = 0f;
        }
    }

    IEnumerator ResetFallFlagAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        hasFallen = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // move with platform
        if (currentPlatform != null)
        {
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;
            controller.Move(platformDelta);
            lastPlatformPosition = currentPlatform.position;
        }

        // fall start
        if (wasGroundedLastFrame && !isGrounded)
        {
            fallStartY = transform.position.y;
        }

        // landing check
        if (!wasGroundedLastFrame && isGrounded && !hasFallen)
        {
            float fallDistance = fallStartY - transform.position.y;
            if (fallDistance >= fatalFallDistance)
            {
                LoseLife();
                return;
            }
        }

        wasGroundedLastFrame = isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // camera move
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

            // rotate to face movement
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
                animator.SetBool("jump1", true);

            PlayJumpSound();
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // animations
        if (animator != null)
        {
            animator.SetBool("run", vertical > 0.1f);
            animator.SetBool("Back run", vertical < -0.1f);

            if (isGrounded && velocity.y <= 0f)
                animator.SetBool("jump1", false);
        }

        // fall of map - lose life
        if (transform.position.y < -5f && !hasFallen)
        {
            LoseLife();
        }
    }

    // platform detection
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.collider.transform)
            {
                currentPlatform = hit.collider.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
        else if (controller.isGrounded)
        {
            currentPlatform = null;
        }
    }

    // lose life logic
    void LoseLife()
    {
        hasFallen = true;
        numLivesLeft--;

        if (numLivesLeft <= 0)
        {
            SceneManager.LoadScene("Level3Lose");
        }
        else
        {
            StartCoroutine(Respawn());
        }
    }

    // respawn logic
    IEnumerator Respawn()
    {
        velocity = Vector3.zero;
        controller.enabled = false;
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.rotation;
        controller.enabled = true;

        yield return new WaitForSeconds(1f);
        hasFallen = false;
    }

    void PlayJumpSound()
    {
        if (jumpSound != null && jumpAudioSource != null)
        {
            float sfxVolume = SoundVolumeSFX.Instance != null ? SoundVolumeSFX.Instance.GetSFXVolume() : 1f;
            jumpAudioSource.volume = sfxVolume;
            jumpAudioSource.PlayOneShot(jumpSound);
        }
    }
}

