using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public GameObject deathEffect;

    public LayerMask wallLayer;
    public LayerMask groundLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayerMask = 1 << other.gameObject.layer;

        if (other.CompareTag("Enemy"))
        {
            EnemyManager enemy = other.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            SpawnEffect();
            Destroy(gameObject);
        }
        else if ((wallLayer.value & otherLayerMask) != 0 || (groundLayer.value & otherLayerMask) != 0)
        {
            SpawnEffect();
            Destroy(gameObject);
        }
    }

    void SpawnEffect()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }
}
