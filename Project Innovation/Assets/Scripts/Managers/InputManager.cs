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
    [SerializeField] private GameEvent _removePartsEvent;
    [SerializeField] private GameEvent _combineWeaponEvent;

    [Header("Components")]
    private PhotonView _photonView;


    public void Setup()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (!_isMainGame.Value)
            SendInput();
    }

    private float GetShakeInput()
    {
        return Input.GetKeyDown(KeyCode.P) ? 2f : Input.acceleration.magnitude;
    }

    public void SendInput()
    {
        if (_isMainGame.Value) return;

        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC("GetInput", RpcTarget.Others, _playerName.Value, _movementInput.Value, _directionInput.Value, _interactingInput.Value, _shootingInput.Value, GetShakeInput());
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void GetInput(string playerName, Vector2 movementInput, Vector2 directionInput, bool interactingInput, bool shootingInput, float shakeInput)
    {
        if (!_isMainGame.Value) return;

        if (!_playersInput.Value.TrySetPlayerInput(playerName, movementInput, directionInput, interactingInput, shootingInput, shakeInput))
        {
            //Failed to find the player
            Debug.Log("Failed");
        }
    }

    //Send data to controller
    public void AddPart(String2 partPickupData)
    {
        string playerName = partPickupData.Item1;
        string partID = partPickupData.Item2;

        _photonView.RPC("AddPartRPC", RpcTarget.Others, playerName, partID);
    }

    [PunRPC]
    public void AddPartRPC(string playerName, string partID)
    {
        if (_playerName.Value != playerName) return;

        WeaponPartData part = GetPartData(partID);

        if (part == null) return;

        _addPartEvent.Raise(part);
    }

    public void RemoveParts(string playerName)
    {
        _photonView.RPC("RemovePartsRPC", RpcTarget.Others, playerName);
    }

    [PunRPC]
    public void RemovePartsRPC(string playerName)
    {
        if (_playerName.Value != playerName) return;

        _removePartsEvent.Raise();
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

    public void CombineWeapon(string playerName)
    {
        _photonView.RPC("CombineWeaponRPC", RpcTarget.Others, playerName);
    }

    [PunRPC]
    public void CombineWeaponRPC(string playerName)
    {
        if (_playerName.Value != playerName) return;

        _combineWeaponEvent.Raise(); 
    }
}