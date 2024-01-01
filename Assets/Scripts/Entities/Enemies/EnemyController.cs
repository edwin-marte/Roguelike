using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float safeDistance = 1f; // The desired safe distance between enemies
    public float avoidanceForce = 5.0f; // The force applied to avoid overlapping enemies

    public float moveSpeed = 3f; // Movement speed of the enemy
    public float stoppingDistance = 0.0f;

    // Shoot
    public bool isShooter = false;
    public Transform weapon;

    //Dash
    public bool canDash = false; // Flag to track if enemy can dash
    public float dashSpeed = 8f; // Dash speed of the enemy
    public float dashCooldown = 0.5f; // Start Cooldown time
    public float postDashCooldown = 5f; // Cooldown time after enemy can dash again
    private bool isDashing = false; // Flag to track if the enemy is dashing
    private Vector2? targetPosition = null;

    private Transform target; // Reference to the player's transform
    private Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component
    private Collider2D[] overlapResults; // Array to store overlap results

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        overlapResults = new Collider2D[5]; // Set the array size based on expected number of overlapping enemies
    }

    private void FixedUpdate()
    {
        if (rb == null || target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget > stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            targetPosition = new Vector2(target.position.x, target.position.y);

            if (!canDash)
            {
                StopChasing();
            }

            if (canDash && !isDashing)
            {
                StopChasing();
                StartDash();
            }

            if (isShooter)
            {
                ShootProjectile();
            }
        }

        FlipCharacter();

        var damageText = transform.Find("DamageText");
        if (damageText != null)
        {
            damageText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void FlipCharacter()
    {
        if (target != null)
        {
            Vector3 directionToPlayer = target.transform.position - transform.position;
            if (directionToPlayer.x > 0)
            {
                // Enemy is to the left of the player, rotate to face the player
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                // Enemy is to the right of the player, rotate to face the player
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void StartDash()
    {
        Vector2? currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2? dashDirection = (targetPosition - currentPosition).Value.normalized;
        rb.velocity = dashDirection.Value * dashSpeed;
        isDashing = true;

        // Set a timer to stop the dash after a specific distance
        StartCoroutine(StopDashAfterDistance());
    }

    private void StopDashing()
    {
        StopChasing();
        isDashing = false;
    }

    private System.Collections.IEnumerator StopDashAfterDistance()
    {
        yield return new WaitUntil(() => Vector2.Distance(transform.position, target.position) <= stoppingDistance && isDashing);

        StartCoroutine(PostDashCooldownTimer());
    }

    private System.Collections.IEnumerator PostDashCooldownTimer()
    {
        yield return new WaitForSeconds(postDashCooldown);
        StopDashing();
    }

    private void ShootProjectile()
    {
        WeaponController weaponController = weapon.GetComponent<WeaponController>();
        weaponController.SetTarget(target.gameObject.GetComponent<Player>());

        weaponController.AimAt(target);
        weaponController.Fire();
    }

    private void ChasePlayer()
    {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        rb.velocity = direction * moveSpeed;

        int overlapCount = Physics2D.OverlapCircleNonAlloc(transform.position, safeDistance, overlapResults);
        for (int i = 0; i < overlapCount; i++)
        {
            Collider2D collider = overlapResults[i];
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                Vector2 desiredDirection = transform.position - collider.transform.position;
                desiredDirection.Normalize();
                rb.velocity += desiredDirection * avoidanceForce * Time.deltaTime;
            }
        }
    }

    private void AvoidOverlappingEnemies()
    {
        // Calculate avoidance vector for overlapping enemies
        Vector2 avoidanceVector = CalculateAvoidanceVector();

        // Calculate desired velocity as current velocity plus the avoidance vector
        Vector2 desiredVelocity = rb.velocity + avoidanceVector * avoidanceForce * Time.fixedDeltaTime;

        // Apply a steering force to gradually adjust velocity towards the desired velocity
        Vector2 steeringForce = (desiredVelocity - rb.velocity) * avoidanceForce * Time.fixedDeltaTime;
        rb.velocity = rb.velocity + steeringForce;

        // Limit the resulting velocity to the enemy's moveSpeed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeed);

        // Update the enemy's position
        rb.MovePosition(rb.position + rb.velocity * Time.fixedDeltaTime);
    }

    private Vector2 CalculateAvoidanceVector()
    {
        int overlapCount = Physics2D.OverlapCircleNonAlloc(transform.position, safeDistance, overlapResults);
        Vector2 avoidanceVector = Vector2.zero;

        for (int i = 0; i < overlapCount; i++)
        {
            Collider2D collider = overlapResults[i];
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                Vector2 separationDirection = (Vector2)transform.position - (Vector2)collider.transform.position;
                separationDirection.Normalize();
                avoidanceVector += separationDirection;
            }
        }

        return avoidanceVector;
    }

    private void StopChasing()
    {
        rb.velocity = Vector2.zero;
    }
}
