using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _deaccceleration = 0.8f;
    [SerializeField] private float _maxSpeed = 50;
    [SerializeField] private float _rotationSpeed = 5;

    private Quaternion _targetRotation;

    [Header("Components")]
    private Rigidbody _rigidbody;
    private CharacterBase _characterBase;

    private void Start()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _characterBase);
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

        Vector2 input = _characterBase.MovementInput.normalized;
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
        Vector2 directionInput = _characterBase.DirectionInput;
        Vector3 direction = new Vector3(directionInput.x, 0f, directionInput.y).normalized;

        if (direction != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }
}