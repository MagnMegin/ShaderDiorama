using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    LayerMask raycastLayers;
    [SerializeField]
    float raycastLength;
    [SerializeField]
    [Range(0f, 1f)]
    float windStrength = 0.5f;


    PlayerInput input;
    InputAction click;
    InputAction altClick;
    InputAction mouseMove;

    bool panning;


    void Start()
    {
        input = new PlayerInput();
        click = input.Default.Click;
        altClick = input.Default.AltClick;
        mouseMove = input.Default.MouseMovement;
        input.Default.Enable();
    }

    void Update()
    {
        if (altClick.WasPerformedThisFrame()) panning = true;
        else if (altClick.WasCompletedThisFrame()) panning = false;

        if (panning)
        {
            Vector2 movement = mouseMove.ReadValue<Vector2>() * 0.1f;
            Quaternion rotation = Quaternion.Inverse(transform.rotation)
                * Quaternion.AngleAxis(movement.x, Vector3.up)
                * transform.rotation
                * Quaternion.AngleAxis(movement.y, Vector3.right);
            transform.localRotation *= rotation;
        }

        if (click.IsPressed())
        {
            TryApplyWInd();
        }
    }

    void TryApplyWInd()
    {
        Camera activeCam = Camera.main;

        Vector2 MousePos = Mouse.current.position.value;

        Ray ray = activeCam.ScreenPointToRay(MousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength, raycastLayers))
        {
            var windTex = hit.collider.GetComponent<WindTexture>();
            Vector3 delta = (Vector3)mouseMove.ReadValue<Vector2>();
            Vector3 xDeltaWorld = activeCam.transform.TransformDirection(delta.x * Vector3.right);
            Vector3 yDeltaWorld = activeCam.transform.TransformDirection(delta.y * Vector3.up);
            if (yDeltaWorld != Vector3.zero)
            {
                float length = yDeltaWorld.sqrMagnitude / Vector3.Cross(hit.normal, yDeltaWorld).magnitude;
                yDeltaWorld = Vector3.ProjectOnPlane(yDeltaWorld, hit.normal).normalized * length;
            }

            Vector2 windVector = new(xDeltaWorld.x + yDeltaWorld.x, xDeltaWorld.z + yDeltaWorld.z);
            windTex.PaintVector(hit.textureCoord, windVector * windStrength);
        }
    }
}
