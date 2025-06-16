using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyManager : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 0.1f;

    public int maxHealth = 3;
    private int currentHealth;

    public GameObject deathEffect;

    private Rigidbody2D rb;
    private int moveDirection;
    public int pointsValue = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        moveDirection = Random.value < 0.5f ? -1 : 1;
        FaceDirection(moveDirection);
    }

    void Update()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * moveDirection, wallCheckDistance, wallLayer);
        if (wallHit.collider != null)
        {
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        moveDirection *= -1;
        FaceDirection(moveDirection);
    }

    void FaceDirection(int dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        PointsManager.Instance.AddPoints(pointsValue);

        Destroy(gameObject);
    }
}
