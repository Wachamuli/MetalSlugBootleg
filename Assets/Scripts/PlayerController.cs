using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform bulletSpawnZone;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private SpriteRenderer upperBodySpriteRenderer;
    [SerializeField] private SpriteRenderer lowerBodySpriteRenderer;

    [Header("Animators")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator upperBodyAnimator;
    [SerializeField] private Animator lowerBodyAnimator;

    [Header("Physics")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float mass = 1.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector2 verticalVelocity;
    private RaycastHit2D boxcast;

    private void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        Controller();
        Jump();
    }

    private void FixedUpdate()
    {
        // Ground check
        boxcast = Physics2D.BoxCast(controller.bounds.center, controller.bounds.size, 0.0f, Vector2.down, 0.0f, groundMask);
        isGrounded = boxcast.collider != null;
    }

    public static Vector3 direction = Vector3.right;
    private void Controller()
    {
        // Right
        if (Input.GetAxisRaw("Horizontal") > 0 && playerSpriteRenderer.flipX)
        {
            InvertXAxisBullestSpawnZone();
            FlipXSprites(false);
            direction = Vector3.right;
        }

        // Left
        if (Input.GetAxisRaw("Horizontal") < 0 && playerSpriteRenderer.flipX == false)
        {
            InvertXAxisBullestSpawnZone();
            FlipXSprites(true);
            direction = Vector3.left;
        }

        // Up
        if (Input.GetAxisRaw("Vertical") >= 0 && isGrounded)
        {
            bulletSpawnZone.localPosition = new Vector3(bulletSpawnZone.localPosition.x, 0.12f, bulletSpawnZone.localPosition.z); ;

            controller.center = new Vector3(0.0f, -0.33f, 0.0f);
            controller.height = 2.0f;

            lowerBodyAnimator.enabled = true;

            if (Input.GetAxisRaw("Vertical") > 0 && isGrounded)
            {
                bulletSpawnZone.localPosition = new Vector3(-0.26f, 3.2f, bulletSpawnZone.localPosition.z);
                direction = Vector3.up;
                upperBodyAnimator.SetBool("isPressingUp", true);
            }
            else
            {
                upperBodyAnimator.SetBool("isPressingUp", false);

                if (playerSpriteRenderer.flipX)
                    direction = Vector3.left;
                else
                    direction = Vector3.right;
            }
        }

        // Down
        if (Input.GetAxisRaw("Vertical") < 0 && isGrounded)
        {
            bulletSpawnZone.localPosition = new Vector3(bulletSpawnZone.localPosition.x, -0.45f, bulletSpawnZone.localPosition.z); ;

            controller.center = new Vector3(0.0f, -0.92f, 0.0f);
            controller.height = 1.2f;

            lowerBodyAnimator.enabled = false;
        }

        var movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        controller.Move(movement * Time.deltaTime * speed);

        playerAnimator.SetBool("isPressingDown", Input.GetAxisRaw("Vertical") < 0 && isGrounded);

        upperBodyAnimator.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        lowerBodyAnimator.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        playerAnimator.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));

    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            upperBodyAnimator.SetBool("isJumping", true);
            lowerBodyAnimator.SetBool("isJumping", true);
        }

        // Reset vertical velocity when it's grounded
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0.0f;
            upperBodyAnimator.SetBool("isJumping", false);
            lowerBodyAnimator.SetBool("isJumping", false);
        }

        // Apply gravity or upward velocity
        verticalVelocity.y += mass * gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }


    private void InvertXAxisBullestSpawnZone()
    {
        var newDirection = new Vector2(bulletSpawnZone.localPosition.x * -1, bulletSpawnZone.localPosition.y);
        bulletSpawnZone.localPosition = newDirection;
    }

    private void FlipXSprites(bool flip)
    {
        playerSpriteRenderer.flipX = flip;
        upperBodySpriteRenderer.flipX = flip;
        lowerBodySpriteRenderer.flipX = flip;
    }
}
