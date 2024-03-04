using Photon.Pun;
using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class InputManager : MonoBehaviour, ISetupManager, IUpdateManager
{
    [Header("Data")]
    [SerializeField] private BoolReference _isMainGame;
    [SerializeField] private PlayersInputReference _playersInput;

    [Header("Input")]
    [SerializeField] private StringReference _playerName;
    [SerializeField] private Vector2Reference _movementInput;
    [SerializeField] private Vector2Reference _directionInput;
    [SerializeField] private BoolReference _interactingInput;
    [SerializeField] private BoolReference _shootingInput;

    [Header("Output")]
    [SerializeField] private List<WeaponPartDataReference> _parts = new List<WeaponPartDataReference>();
    [SerializeField] private WeaponPartDataReference _addPartEvent;

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
            _photonView.RPC("GetInput", RpcTarget.Others, _playerName.Value, _movementInput.Value, _directionInput.Value, _interactingInput.Value, _shootingInput.Value);
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void GetInput(string playerName, Vector2 movementInput, Vector2 directionInput, bool interactingInput, bool shootingInput)
    {
        if (!_isMainGame.Value) return;

        if (!_playersInput.Value.TrySetPlayerInput(playerName, movementInput, directionInput, interactingInput, shootingInput))
        {
            //Failed to find the player
            Debug.Log("Failed");
        }
    }

    //Send data to controller
    public void AddPart(string playerName, string partID)
    {
        _photonView.RPC("GetInput", RpcTarget.Others, playerName, partID);
    }

    [PunRPC]
    public void AddPartRPC(string playerName, string partID)
    {
        if (_playerName.Value != playerName) return;

        WeaponPartData part = GetPartData(partID);

        if (part == null) return;

        _addPartEvent.Raise(part);
    }

    private WeaponPartData GetPartData(string partID)
    {
        for(int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].Value.ID == partID)
                return _parts[i].Value;
        }

        return null;
    }
}