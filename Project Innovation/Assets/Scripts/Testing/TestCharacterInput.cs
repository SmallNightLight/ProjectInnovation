using ScriptableArchitecture.Data;
using UnityEngine;

public class TestCharacterInput : MonoBehaviour
{
    [SerializeField] private PlayerInputReference _testPlayerInput;

    [Header("Input")]
    [SerializeField] private Vector2Reference _movementInput;
    [SerializeField] private Vector2Reference _directionInput;
    [SerializeField] private BoolReference _interactingInput;
    [SerializeField] private BoolReference _shootingInput;

    private void Update()
    {
        _testPlayerInput.Value.MovementInput = _movementInput.Value;
        _testPlayerInput.Value.DirectionInput = _directionInput.Value;
        _testPlayerInput.Value.InteractingInput = _interactingInput.Value;
        _testPlayerInput.Value.ShootingInput = _shootingInput.Value;
    }
}