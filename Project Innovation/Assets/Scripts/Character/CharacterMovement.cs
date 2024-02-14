using ScriptableArchitecture.Data;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Vector2Reference _joyconInput;

    [Header("Settings")]
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _maxSpeed = 50;

    [Header("Components")]
    private Rigidbody _rigidbody;

    private void Start()
    {
        TryGetComponent(out _rigidbody);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SetMovement();
    }

    private void SetMovement()
    {
        if (_rigidbody == null)
        {
            Debug.LogWarning("Cannot apply movement to character due to missing rigidbody");
            return;
        }

        Vector2 input = _joyconInput.Value.normalized;
        Vector3 inputVelocity = new Vector3(input.y, 0, -input.x);

        if (input.magnitude > 0)
        {
            _rigidbody.AddForce(_speed * inputVelocity, ForceMode.Acceleration);
        }
        else
        {
            _rigidbody.velocity *= 0.9f;
        }

        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }
}