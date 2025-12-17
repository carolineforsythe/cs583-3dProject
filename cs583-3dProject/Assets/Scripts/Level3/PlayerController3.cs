using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3 : MonoBehaviour
{
    public Transform RespawnPoint;
    private bool canTakeDamage = true;
    private bool hasFallen = false;

    public float moveSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    public Transform cameraTransform;
    public Animator animator;

    public int maxHealth = 3;
    public static int numLivesLeft = 3;

    [Header("Fall Death")]
    public float fatalFallDistance = 2f;
    public string loseSceneName = "Level3Lose"; 

    // jump sound
    public AudioSource jumpAudioSource;
    public AudioClip jumpSound;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int currentHealth;

    // fall tracking
    private float fallStartY;
    private bool wasGroundedLastFrame;

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
        Debug.Log("Fall flag reset");
    }


    public void LoseLifeAndRespawn()
    {
        if (hasFallen) return;

        hasFallen = true;
        numLivesLeft--;
        Debug.Log("Lives remaining: " + numLivesLeft);

        if (numLivesLeft <= 0)
        {
            SceneManager.LoadScene(loseSceneName);
            return;
        }

        velocity = Vector3.zero;
        controller.enabled = false;
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.rotation;
        controller.enabled = true;

        StartCoroutine(ResetFallFlagAfterDelay());
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (wasGroundedLastFrame && !isGrounded)
        {
            fallStartY = transform.position.y;
        }

        if (!wasGroundedLastFrame && isGrounded && !hasFallen)
        {
            float fallDistance = fallStartY - transform.position.y;
            if (fallDistance >= fatalFallDistance)
            {
                LoseLifeAndRespawn();
                return;
            }
        }

        wasGroundedLastFrame = isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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

            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetBool("jump1", true);
            }

            PlayJumpSound();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            bool movingForward = vertical > 0.1f;
            bool movingBackward = vertical < -0.1f;


            if (!isGrounded)
            {
                animator.SetBool("jump1", true);
                animator.SetBool("run", false);
                animator.SetBool("Back run", false);
            }
            else
            {
                animator.SetBool("jump1", false);
                animator.SetBool("run", movingForward);
                animator.SetBool("Back run", movingBackward);
            }
        }

        if (transform.position.y < -5f && !hasFallen)
        {
            LoseLifeAndRespawn();
        }
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
