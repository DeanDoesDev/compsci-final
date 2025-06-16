using System.Collections;
using UnityEngine;
using TMPro;

public class SimpleShooter : MonoBehaviour
{
    public Gun[] guns;
    public Gun currentGun;

    public Transform firePoint;
    public float bulletSpeed = 10f;

    private float fireCooldown = 0f;
    private int currentGunIndex = 0;

    public float gunSwitchInterval = 5f;
    private float gunSwitchTimer;

    public GameObject shootEffect;
    public Transform shootEffectPos;

    public TextMeshProUGUI gunNameText;

    void Start()
    {
        if (guns != null && guns.Length > 0)
        {
            currentGun = guns[0];
            currentGunIndex = 0;
        }

        gunSwitchTimer = gunSwitchInterval;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && fireCooldown <= 0f && currentGun != null)
        {
            Shoot();
            fireCooldown = currentGun.fireRate;
        }

        gunSwitchTimer -= Time.deltaTime;
        if (gunSwitchTimer <= 0f)
        {
            SwitchToRandomGun();
            gunSwitchTimer = gunSwitchInterval;
        }
    }

    void Shoot()
    {
        ScreenShake.instance.TriggerShake(0.05f, 0.05f, 1f);

        int count = Mathf.Max(1, currentGun.bulletCount);
        for (int i = 0; i < count; i++)
        {
            float angleOffset = Random.Range(-currentGun.spreadAngle, currentGun.spreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, angleOffset);
            Vector2 direction = spreadRotation * (transform.right * Mathf.Sign(transform.localScale.x));

            GameObject bullet = Instantiate(currentGun.bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().damage = currentGun.bulletDamage;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            rb.velocity = direction.normalized * bulletSpeed;
        }

        GameObject eff = Instantiate(shootEffect, shootEffectPos.position, Quaternion.identity);

        Destroy(eff, 2f);
    }

    public void SwitchToNextGun()
    {
        currentGunIndex++;
        if (currentGunIndex >= guns.Length)
            currentGunIndex = 0;
        ApplyGun(currentGunIndex);
    }

    public void SwitchToPreviousGun()
    {
        currentGunIndex--;
        if (currentGunIndex < 0)
            currentGunIndex = guns.Length - 1;
        ApplyGun(currentGunIndex);
    }

    public void SwitchToGun(int index)
    {
        if (index >= 0 && index < guns.Length)
        {
            currentGunIndex = index;
            ApplyGun(currentGunIndex);
        }
    }

    public void SwitchToRandomGun()
    {
        if (guns.Length <= 1) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, guns.Length);
        } while (newIndex == currentGunIndex);

        SwitchToGun(newIndex);
    }

    private void ApplyGun(int index)
    {
        currentGun = guns[index];

        if (gunNameText != null && currentGun != null)
        {
            gunNameText.text = currentGun.name.ToLower();
        }
    }
}
