using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSystem : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float raycastRange;

    RaycastHit latestHit;
    GameObject currentTarget;

    public GameObject CurrentTarget => currentTarget;
    public Action<GameObject> TargetChanged { get; private set; }

    private void Update()
    {
        HandleScreenRaycast();
    }

    void HandleScreenRaycast()
    {
        Camera activeCam = Camera.main;

        Vector2 MousePos = Mouse.current.position.value;
        Ray ray = activeCam.ScreenPointToRay(MousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, targetLayers))
        {
            HoverTarget(hit.collider.gameObject);
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            ResetTarget();
            Debug.DrawRay(ray.origin, ray.direction * raycastRange, Color.yellow);
        }
    }

    void HoverTarget(GameObject target)
    {
        if (target == currentTarget) return;

        currentTarget = target;
        TargetChanged?.Invoke(currentTarget);
    }

    void ResetTarget()
    {
        currentTarget = null;
        TargetChanged?.Invoke(currentTarget);
    }
}
