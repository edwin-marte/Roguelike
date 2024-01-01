using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float damping = 0.125f; // The smoothness of the camera movement
    [SerializeField] private Vector2 offset; // The offset between the camera and the target

    private Transform target; // The target to follow
    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z);
            targetPosition.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}
