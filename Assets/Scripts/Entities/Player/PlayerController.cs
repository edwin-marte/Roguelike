using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform visuals;
    [SerializeField] private float moveSpeed = 7;
    private Collider2D playableAreaCollider; // Assign the playable area collider in the Inspector
    private Rigidbody2D rb;
    private Vector3 direction;
    private Vector3 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playableAreaObject = GameObject.FindWithTag("Limits");

        if (playableAreaObject != null)
        {
            playableAreaCollider = playableAreaObject.GetComponent<Collider2D>();
        }
    }

    private void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        Vector3 moveInput = Vector2.zero;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        direction = moveInput.normalized;
        movement = direction * moveSpeed * Time.fixedDeltaTime;

        if (moveInput.x > 0)
        {
            visuals.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (moveInput.x < 0)
        {
            visuals.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void Move()
    {
        Vector3 newPosition = transform.position + movement;

        // Clamp the newPosition within the playable area
        Vector3 clampedPosition = playableAreaCollider.bounds.ClosestPoint(newPosition);
        rb.MovePosition(clampedPosition);
    }
}
