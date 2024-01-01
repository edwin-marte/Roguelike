using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class WeaponController : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform firePoint; // The position where the projectile is fired

    private Weapon weapon;
    private Entity target;
    private bool canShoot = true;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    public void AimAt(Transform aimTarget)
    {
        // Rotate the weapon to aim at the target
        Vector2 direction = aimTarget.position - transform.position;
        transform.up = direction.normalized;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void ResetRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 10f * Time.deltaTime);
    }

    public void Fire()
    {
        if (!canShoot)
            return;

        StartCoroutine(SpawnBulletsWithCooldown());

        // Check if it's the player who is shooting
        if (this.GetComponentInParent<Player>() != null)
        {
            // Play hit effect
            //AudioManager.Instance.PlayShotSFX(AudioData.Instance.shot);
            AudioManager.PlaySound(AudioManager.Sound.PlayerShot);
        }

        canShoot = false;
        target = null;
        Invoke("EnableShooting", weapon.GetWeaponStat(WeaponStat.Cooldown));
    }

    private IEnumerator SpawnBulletsWithCooldown()
    {
        for (int i = 0; i < weapon.GetWeaponStat(WeaponStat.Amount_Of_Bullets); ++i)
        {
            // Instantiate the projectile at the fire point position
            var bullet = Instantiate(weapon.GetProjectile(), firePoint.position, firePoint.rotation);
            bullet.SetDamage(weapon.GetWeaponStat(WeaponStat.Damage));
            bullet.SetSpeed(weapon.GetWeaponStat(WeaponStat.Bullet_Speed));

            yield return new WaitForSeconds(0.2f); // Wait for cooldown before next bullet
        }
    }

    public void SetTarget(Entity enemy)
    {
        target = enemy;
    }

    public Entity GetTarget()
    {
        return target;
    }

    public void SetCanShoot(bool cShoot)
    {
        canShoot = cShoot;
    }

    public bool CanShoot()
    {
        return canShoot;
    }

    private void EnableShooting()
    {
        canShoot = true;
    }
}
