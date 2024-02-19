using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterInput))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _deaccceleration = 0.8f;
    [SerializeField] private float _maxSpeed = 50;
    [SerializeField] private float _rotationSpeed = 5;

    [Header("Components")]
    private Rigidbody _rigidbody;
    private CharacterInput _characterInput;

    private void Start()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _characterInput);
    }

    private void FixedUpdate()
    {
        SetMovement();
        SetDirection();
    }

    private void SetMovement()
    {
        if (_rigidbody == null)
        {
            Debug.LogWarning("Cannot apply movement to character due to missing rigidbody");
            return;
        }

        Vector2 input = _characterInput.MovementInput.normalized;
        Vector3 inputVelocity = new Vector3(-input.y, 0, input.x);

        if (input.magnitude > 0)
        {
            _rigidbody.AddForce(_acceleration * inputVelocity, ForceMode.Acceleration);
        }
        else
        {
            _rigidbody.velocity *= _deaccceleration;
        }

        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }
   
    private void SetDirection()
    {
        Vector2 directionInput = _characterInput.DirectionInput;
        Vector3 direction = new Vector3(directionInput.x, 0f, directionInput.y).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}