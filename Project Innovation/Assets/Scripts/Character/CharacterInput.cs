using ScriptableArchitecture.Data;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private PlayersInputReference _playersInput;
    [SerializeField] private bool _useKeyboardInput;
    [SerializeField] private Camera _camera;

    private PlayerInput _playerInput;

    private void Start()
    {
        if (!_useKeyboardInput)
        {
            InitializeCharacter("TestCharacter");
        }
    }

    public void InitializeCharacter(string playerName)
    {
        _playersInput.Value.TryGetPlayerInput(playerName, out _playerInput);
    }

    public PlayerInput CurrentInput
    {
        get
        {
            return new PlayerInput()
            {
                MovementInput = MovementInput,
                DirectionInput = DirectionInput,
                InteractingInput = InteractingInput,
                ShootingInput = ShootingInput
            };
        }
    }

    public Vector2 MovementInput
    {
        get
        {
            if (_useKeyboardInput)
            {
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            return _playerInput.MovementInput;
        }
    }

    public Vector2 DirectionInput
    {
        get
        {
            if (_useKeyboardInput)
            {
                Vector3 mousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y));
                Vector3 direction = mousePosition - transform.position;
                return new Vector2(direction.x, direction.z);
            }

            return _playerInput.DirectionInput;
        }
    }

    public bool InteractingInput
    {
        get
        {
            if (_useKeyboardInput)
            {
                return Input.GetMouseButton(1);
            }

            return _playerInput.InteractingInput;
        }
    }

    public bool ShootingInput
    {
        get
        {
            if (_useKeyboardInput)
            {
                return Input.GetMouseButton(0);
            }

            return _playerInput.ShootingInput;
        }
    }
}