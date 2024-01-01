using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public List<WeaponController> startingWeapons; // List of starting weapons
    public List<GameObject> weaponSlots;
    public LayerMask enemyLayer; // Layer mask for enemies
    public float detectionRadius = 5f;

    private List<WeaponController> weapons; // List of all weapons
    private Collider2D[] overlappingColliders; // Array to store overlapping colliders
    private HashSet<Enemy> targetedEnemies; // Set of currently targeted enemies

    public void SetupStartingWeapons()
    {
        weapons = new List<WeaponController>();
        targetedEnemies = new HashSet<Enemy>();

        for (int i = 0; i < startingWeapons.Count; i++)
        {
            var slot = weaponSlots[i];
            var weapon = Instantiate(startingWeapons[i], slot.transform.position, Quaternion.identity, slot.transform);
            weapons.Add(weapon);
        }
    }

    private void Awake()
    {
        overlappingColliders = new Collider2D[10]; // Adjust the size based on expected maximum overlapping colliders
    }

    private void Update()
    {
        targetedEnemies.Clear();

        foreach (WeaponController weapon in weapons)
        {
            Enemy target = FindTarget(weapon);

            if (target == null || targetedEnemies.Contains(target))
                continue;

            weapon.SetTarget(target);

            if (weapon.CanShoot())
            {
                weapon.AimAt(target.transform);
                weapon.Fire();
            }
            else
            {
                weapon.AimAt(target.transform);
            }

            targetedEnemies.Add(target);
        }
    }

    private Enemy FindTarget(WeaponController weapon)
    {
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, overlappingColliders, enemyLayer);

        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < numColliders; i++)
        {
            Enemy enemy = overlappingColliders[i].GetComponent<Enemy>();
            if (targetedEnemies.Contains(enemy))
                continue;

            float distance = Vector2.Distance(weapon.transform.position, enemy.transform.position);

            if (distance <= detectionRadius && distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
