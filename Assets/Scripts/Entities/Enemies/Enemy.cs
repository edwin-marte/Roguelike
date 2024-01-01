using UnityEngine;

public class Enemy : Entity
{
    public float hpWave = 2f;
    public float damage = 1f;
    public float collectibleSpawnProbability = 0.5f;
    public GameObject soulPrefab;
    public GameObject bloodSplash;

    //Animations
    public Animator animator;
    public Animator animator2;
    public Animator animator3;

    private GameObject soul;
    private bool isTargeted = false;

    public bool IsTargeted()
    {
        return isTargeted;
    }

    public void SetTargeted(bool value)
    {
        isTargeted = value;
    }

    public void ResetTargetedStatus()
    {
        isTargeted = false;
    }

    public override void TookDamage()
    {
        //Show animations
        if (animator != null && animator2 != null && animator3 != null)
        {
            animator.SetTrigger("TakeHit");
            animator2.SetTrigger("TakeHit");
            animator3.SetTrigger("TakeHit");
        }

        // Play hit effect
        AudioManager.PlaySound(AudioManager.Sound.EnemyHit);

        // Instantiate the hit effect
        GameObject deathEffect = Instantiate(bloodSplash, transform.position, Quaternion.Euler(-115f, 0f, 0f));
        Destroy(deathEffect, 1f);
    }

    public override void Die()
    {
        base.Die();

        if (Random.value <= collectibleSpawnProbability)
        {
            // Spawn the collectible item at the enemy's position with random z-axis rotation
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            soul = Instantiate(soulPrefab, transform.position, randomRotation);
        }
    }
}
