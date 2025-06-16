using UnityEngine;

[CreateAssetMenu(menuName = "Gun")]
public class Gun : ScriptableObject
{
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public float spreadAngle = 3f;
    public int bulletCount = 1;
    public int bulletDamage = 1;
}
