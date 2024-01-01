using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] private float baseHealth = 3f; // Initial health of the object
    private float startingHealth = 100f; // Initial health of the object

    private float currentHealth = 0f;

    protected virtual void Start()
    {
        currentHealth = baseHealth;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        TookDamage();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void TookDamage() { }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void SetStartingHealth(float amount)
    {
        this.startingHealth = amount;
        this.currentHealth = this.startingHealth;
    }

    public float GetBaseHealth()
    {
        return baseHealth;
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}