using ScriptableArchitecture.Data;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayersInputReference _playersInput;
    [SerializeField] private PlayerCharactersReference _characterAssignment;
    [SerializeField] private CharacterCollectionReference _characterCollection;
    private CharacterData _characterData;
    private string _playerName;
    private string _characterName;

    [Header("Settings")]
    [SerializeField] private InputType _useKeyboardInput;
    [SerializeField] private int _team;
    [SerializeField] private Vector3 _spawnPosition;

    [Header("Components")]
    [SerializeField] private Camera _camera;

    [Header("scriptData")]
    private PlayerInput _playerInput;

    private enum InputType
    {
        Network,
        Keyboard,
        None
    }

    public void InitializeCharacter(string playerName, int team, Vector3 spawnPosition)
    {
        _playerName = playerName;
        _characterName = _characterAssignment.Value.GetCharacter(playerName);

        if (_characterName == "") _characterName = "Piggy";

        _characterCollection.Value.TryGetCharacter(_characterName, out _characterData);

        if (_characterData == null) Debug.LogWarning("No character data found");

        _playersInput.Value.TryGetPlayerInput(playerName, out _playerInput);
        _team = team;
        _spawnPosition = spawnPosition;

        //Add character model
        Instantiate(_characterData.GameCharacter, transform);

        //Initilaize health bar
        if (TryGetComponent(out CharacterHealth characterHealth))
        {
            characterHealth.Initialize(team, _characterData);
        }
    }

    public int Team
    {
        get
        {
            return _team;
        }
    }

    public string PlayerName
    {
        get
        {
            return _playerName;
        }
    }

    public string CharacterName
    {
        get
        {
            return _characterName;
        }
    }

    public CharacterData CharacterData
    {
        get
        {
            return _characterData;
        }
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
            switch (_useKeyboardInput)
            {
                case InputType.Network:
                    return _playerInput.MovementInput;
                case InputType.Keyboard:
                    return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                default:
                    return new Vector2(0, 0);
            }
        }
    }

    public Vector2 DirectionInput
    {
        get
        {
            switch (_useKeyboardInput)
            {
                case InputType.Network:
                    return new Vector3(-_playerInput.DirectionInput.y, _playerInput.DirectionInput.x);
                case InputType.Keyboard:
                    Vector3 mousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y));
                    Vector3 direction = mousePosition - transform.position;
                    return new Vector2(direction.x, direction.z);
                default:
                    return new Vector2(0, 0);
            }
        }
    }

    public bool InteractingInput
    {
        get
        {
            switch (_useKeyboardInput)
            {
                case InputType.Network:
                    return _playerInput.InteractingInput;
                case InputType.Keyboard:
                    return Input.GetMouseButton(1);
                default:
                    return false;
            }
        }
    }

    public bool ShootingInput
    {
        get
        {
            switch (_useKeyboardInput)
            {
                case InputType.Network:
                    return _playerInput.ShootingInput;
                case InputType.Keyboard:
                    return Input.GetMouseButton(0);
                default:
                    return false;
            }
        }
    }

    public float ShakeInput
    {
        get
        {
            switch (_useKeyboardInput)
            {
                case InputType.Network:
                    return _playerInput.ShakeInput;
                case InputType.Keyboard:
                    return Input.GetMouseButton(2) ? 2f : 0f;
                default:
                    return 0;
            }
        }
    }

    public Vector3 SpawnPosition
    {
        get
        {
            return _spawnPosition;
        }
    }
}