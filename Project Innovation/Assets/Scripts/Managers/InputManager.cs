using Photon.Pun;
using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class InputManager : MonoBehaviour, ISetupManager, IUpdateManager
{
    [Header("Data")]
    [SerializeField] private BoolReference _isMainGame;
    [SerializeField] private PlayersInputReference _playersInput;

    [Header("Input")]
    [SerializeField] private Vector2Reference _movementInput;
    [SerializeField] private Vector2Reference _directionInput;
    [SerializeField] private BoolReference _interactingInput;
    [SerializeField] private BoolReference _shootingInput;

    [Header("Prefabs")]
    [SerializeField] private GameObject _characterPrefab;

    [Header("Components")]
    private PhotonView _photonView;


    public void Setup()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        SendInput();
    }

    public void SendInput()
    {
        if (_isMainGame.Value) return;

        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC("GetInput", RpcTarget.Others, _photonView.OwnerActorNr, _movementInput.Value, _directionInput.Value, _interactingInput.Value, _shootingInput.Value);
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void GetInput(int player, Vector2 movementInput, Vector2 directionInput, bool interactingInput, bool shootingInput)
    {
        if (!_isMainGame.Value) return;

        //Maybe dont use the player number for name
        string playerName = player.ToString();
        
        if (!_playersInput.Value.TrySetPlayerInput(playerName, movementInput, directionInput, interactingInput, shootingInput))
        {
            //Failed to find the player, so add a new player
            AddNewPlayer(playerName);
        }

        Debug.Log($"Player: {player}\nMovement: {movementInput}\nDirectionInput: {directionInput}\n Interacting: {interactingInput}\nShooting: {shootingInput}");
    }

    public void AddNewPlayer(string playerName)
    {
        Debug.Log("Added player");

        //Create new character gameObject
        Instantiate(_characterPrefab);
        
         //Add player data   
        _playersInput.Value.AddNewPlayer(playerName);
    }
}