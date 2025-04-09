using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InverseColliderRestraint : MonoBehaviour
{
    [SerializeField]
    new Collider collider;
    [SerializeField]
    float escapeThreshold = 0.1f;
    [SerializeField]
    float correctionAmount = 0.5f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 colliderPoint = collider.ClosestPoint(transform.position);
        Vector3 rbOffset = (colliderPoint - transform.position);
        float colliderSqrDistance = rbOffset.sqrMagnitude;

        if (colliderSqrDistance > escapeThreshold * escapeThreshold)
        {
            rb.position = colliderPoint + rbOffset.normalized * correctionAmount;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
