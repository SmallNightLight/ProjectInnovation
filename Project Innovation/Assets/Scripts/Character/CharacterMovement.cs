using ScriptableArchitecture.Data;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayersInputReference _playersInput;

    [Header("Testing")]
    [SerializeField] private bool _useCustomInput;
    [SerializeField] private PlayerInputReference _testInput;

    [Header("Settings")]
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _deaccceleration = 0.8f;
    [SerializeField] private float _maxSpeed = 50;

    [Header("Other")]
    [SerializeField] private string _playerName;
    private PlayerInput _playerInput;

    [Header("Components")]
    private Rigidbody _rigidbody;

    private void Start()
    {
        TryGetComponent(out _rigidbody);

        if (_useCustomInput)
        {
            InitializeCharacter("TestCharacter");
        }
    }

    private void FixedUpdate()
    {
        if (_playerInput != null)
        {
            DoMovement();
        }
    }

    private void DoMovement()
    {
        if (_rigidbody == null)
        {
            Debug.LogWarning("Cannot apply movement to character due to missing rigidbody");
            return;
        }

        Vector2 input = _playerInput.MovementInput.normalized;
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

    public void InitializeCharacter(string playerName)
    {
        _playerName = playerName;

        if (_useCustomInput)
        {
            _playerInput = _testInput.Value;
        }
        else
        {
            _playersInput.Value.TryGetPlayerInput(_playerName, out _playerInput);
        }
    }
}