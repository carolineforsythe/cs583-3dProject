using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public Transform cameraTransform;
    public Animator animator;      
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
        // check ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // check input
        float horizontal = Input.GetAxis("Horizontal");   
        float vertical   = Input.GetAxis("Vertical");     

        // move camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right   = cameraTransform.right;
        forward.y = 0f;
        right.y   = 0f;
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

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);


            if (animator != null)
            {
                animator.SetBool("jump1", true);
            }
        }

        // gravity 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // animations
        if (animator != null)
        {

            bool movingForward  = vertical >  0.1f;
            bool movingBackward = vertical < -0.1f;

            // forward movement
            animator.SetBool("run", movingForward);

            // backwards
            animator.SetBool("Back run", movingBackward);
           

            // jumping
            if (isGrounded && velocity.y <= 0f)
            {
                animator.SetBool("jump1", false);
            }

            
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // die
            if (animator != null)
                animator.SetBool("die", true);

            // restart scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}


