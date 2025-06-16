using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlatformerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public GameObject ghostPrefab;
    public float ghostInterval = 0.05f;         

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount;
    public int maxJumps = 2;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTime;
    private float ghostTimer;

    private int moveDirection = 0; 
    private bool leftHeld, rightHeld;

    private int faceOverride = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        leftHeld = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        rightHeld = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (leftHeld && !rightHeld)
            moveDirection = -1;
        else if (rightHeld && !leftHeld)
            moveDirection = 1;
        else if (!leftHeld && !rightHeld)
            moveDirection = 0;

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            FaceDirection(1);
            faceOverride = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            FaceDirection(-1);
            faceOverride = -1;
        }

        if (faceOverride != 0)
        {
            bool overrideKeyHeld = (faceOverride == -1 && leftHeld) || (faceOverride == 1 && rightHeld);
            if (!overrideKeyHeld)
            {
                if (moveDirection != 0)
                {
                    FaceDirection(moveDirection);
                }
                faceOverride = 0;
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && (coyoteTimeCounter > 0 || jumpCount < maxJumps - 1) && !isDashing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            jumpBufferCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            ScreenShake.instance.TriggerShake(0.2f, 0.1f, 1f);

            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashTime = dashDuration;
        ghostTimer = 0f;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float direction = Mathf.Sign(transform.localScale.x);

        while (dashTime > 0)
        {
            rb.velocity = new Vector2(direction * dashSpeed, 0f);

            ghostTimer -= Time.deltaTime;
            if (ghostTimer <= 0f)
            {
                SpawnGhost();
                ghostTimer = ghostInterval;
            }

            dashTime -= Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost.transform.localScale = transform.localScale;
    }

    void FaceDirection(int dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }
}
