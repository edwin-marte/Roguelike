using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float pickupRange = 2f;
    public float attractionSpeed = 10f;
    public LayerMask collectibleLayer; // Layer to detect objects

    private UIPlayer uiPlayer;
    private ResourceManager resourceManager;
    private SpriteRenderer playerSpriteRenderer;
    private Color originalColor;
    private float timer = 0f;
    private float incrementInterval = 0.2f;
    private InGameManager inGameManager;

    private HashSet<Transform> detectedObjects = new HashSet<Transform>(); // Cache detected objects

    private void Awake()
    {
        //playerSpriteRenderer = GetComponent<SpriteRenderer>();
        //originalColor = playerSpriteRenderer.color;
        timer = incrementInterval;
    }

    protected override void Start()
    {
        base.Start();
        inGameManager = InGameManager.Instance;
        resourceManager = ResourceManager.Instance;
        uiPlayer = UIPlayer.Instance;
        uiPlayer.SetMaxHealth(GetBaseHealth(), GetCurrentHealth());
    }

    private void Update()
    {
        DetectCollectibles();
        AttractCollectibles();
    }

    private void DetectCollectibles()
    {
        detectedObjects.Clear();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange, collectibleLayer);

        foreach (Collider2D collider in colliders)
        {
            detectedObjects.Add(collider.transform);
        }
    }

    private void AttractCollectibles()
    {
        Vector2 targetPosition = transform.position;
        float step = attractionSpeed * Time.deltaTime;

        foreach (Transform detectedObject in detectedObjects)
        {
            Vector2 currentPosition = detectedObject.position;
            Transform objectTransform = detectedObject.transform;
            Vector2 newPosition = Vector2.Lerp(currentPosition, targetPosition, step);
            objectTransform.position = newPosition;

            if (Vector2.Distance(newPosition, targetPosition) <= 0.01f)
            {
                detectedObjects.Remove(detectedObject);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collectibleLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            resourceManager.AddSouls(1f);
            // Play collect effect
            AudioManager.PlaySound(AudioManager.Sound.CollectSoul);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //playerSpriteRenderer.color = Color.red;
            Enemy enemy = collider.GetComponent<Enemy>();

            timer += Time.deltaTime;

            if (timer >= incrementInterval)
            {
                TakeDamage(enemy.damage);
                uiPlayer.SetHealth(GetCurrentHealth(), GetBaseHealth());
                timer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //playerSpriteRenderer.color = originalColor;
            timer = incrementInterval;
        }
    }

    public override void Die()
    {
        inGameManager.GameOver();
        base.Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
