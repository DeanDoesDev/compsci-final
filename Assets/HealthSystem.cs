using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            GameManager.Instance.Die();
        }
    }
}
