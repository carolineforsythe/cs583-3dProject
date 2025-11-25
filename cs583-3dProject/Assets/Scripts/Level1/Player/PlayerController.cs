using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public Transform cameraTransform; 
    public Animator animator; // drag monkey Animator here 
    public int maxHealth = 3;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int currentHealth;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Movement relative to camera
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

            // Rotate monkey to face movement direction
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation parameters (optional for later)
        if (animator != null)
        {
            float speedPercent = move.magnitude;
            animator.SetFloat("Speed", speedPercent);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", velocity.y);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // Restart current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

