using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TargetSystem))]
public class ObjectManipulator : MonoBehaviour
{
    public enum State
    {
        Disabled,
        Move,
        Rotate,
    }

    [SerializeField]
    private Rigidbody manipulationTarget;

    TargetSystem targetSystem;
    
    PlayerInput input;
    InputAction grab;
    InputAction mouseMove;

    State state;

    float queuedRotation;

    void Start()
    {
        targetSystem = GetComponent<TargetSystem>();

        input = new PlayerInput();
        grab = input.Default.Click;
        mouseMove = input.Default.MouseMovement;
        input.Default.Enable();
    }

    void Update()
    {
        if (grab.WasPerformedThisFrame())
        {
            if (targetSystem.CurrentTarget != null)
            {
                state = State.Move;
            }
            else
            {
                state = State.Rotate;
            }
        }
        else if (grab.WasCompletedThisFrame())
        {
            state = State.Disabled;
        }

        Vector2 mouseInput = mouseMove.ReadValue<Vector2>();
        switch (state)
        {
            case State.Disabled:
                break;
            case State.Move:
                MoveInPlane(mouseInput);
                break;
            case State.Rotate:
                Rotate(mouseInput);
                break;
        }
    }

    private void FixedUpdate()
    {
        var rotation = Quaternion.Euler(0f, queuedRotation, 0f);
        manipulationTarget.MoveRotation(manipulationTarget.rotation * rotation);
        queuedRotation = 0f;
    }

    void MoveInPlane(Vector2 input)
    {
        manipulationTarget.MovePosition(manipulationTarget.position + (Vector3)input * 0.1f);
    }

    void Rotate(Vector2 input)
    {
        queuedRotation += input.x;
    }
}
