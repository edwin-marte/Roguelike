using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 0.2f; // Projectile lifetime in seconds
    public float explosionRadius = 0.7f; // Radius of the explosion
    public float explosionVisualDuration = 0.2f; // Duration the explosion visual is shown
    public LayerMask damageableLayers; // Layers that can be damaged
    public bool shouldExplode = false; // Bool to determine if the projectile should explode
    public GameObject explosionVisualPrefab; // Prefab for the explosion visual
    public Vector2 explosionVisualSize = new Vector2(1.0f, 1.0f); // Size of the explosion visual

    private float weaponDamage = 0.0f;
    private float speed = 0.0f; // Projectile speed

    private void Update()
    {
        // Move the projectile in its forward direction
        transform.position += transform.up * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (shouldExplode)
            {
                Explode();
            }
            else
            {
                damageable.TakeDamage(weaponDamage);
            }
            Destroy(this.gameObject, lifetime);
        }
    }

    private void Explode()
    {
        // Instantiate the explosion visual
        GameObject explosionVisual = Instantiate(explosionVisualPrefab, transform.position, Quaternion.identity);

        // Detect nearby colliders within the explosion radius
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius, damageableLayers);

        // Apply damage to all detected colliders
        foreach (Collider2D enemy in enemiesToHit)
        {
            // Check if the collider has a health component
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(weaponDamage);
            }
        }

        Destroy(explosionVisual, explosionVisualDuration);
    }

    public void SetDamage(float damage)
    {
        this.weaponDamage = damage;
    }

    public float GetDamage()
    {
        return this.weaponDamage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the Gizmos color

        // Draw the explosion radius using a wire sphere
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

