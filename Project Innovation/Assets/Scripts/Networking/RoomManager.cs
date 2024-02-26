using UnityEngine;
using Photon.Pun;
using TMPro;
using ScriptableArchitecture.Data;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;

    [SerializeField] private BoolReference _createDefaultRoom;
    [SerializeField] private StringReference _gameSceneName;
    [SerializeField] private StringReference _defaultRoomName;

    [SerializeField] private StringReference _playerName;
    [SerializeField] private BoolReference _inTeamLobby;
    [SerializeField] private StringReference _UIMessageEvent;
    [SerializeField] private GameEvent _joinedRoomEvent;

    [Header("Settings")]
    [SerializeField] private BoolReference _isMainGame;

    [Header("Components")]
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private TMP_InputField _playerNameInput;


    public void Setup()
    {
        if (_createDefaultRoom.Value)
        {
            CreateDefaultRoom();
        }
    }

    private void CreateDefaultRoom()
    {
        SetPlayerName();

        PhotonNetwork.CreateRoom(_defaultRoomName.Value);
    }

    public void CreateRoom()
    {
        SetPlayerName();

        PhotonNetwork.CreateRoom(_roomNameInput.text);
    }

    public void JoinRoom()
    {
        SetPlayerName();

        PhotonNetwork.JoinRoom(_roomNameInput.text);
    }

    public void LeaveRoom()
    {
        _UIMessageEvent.Raise($"Room already has a player with the name: {_playerName.Value}");
        Debug.Log($"Room already has a player with the name: {_playerName.Value}");

        PhotonNetwork.LeaveRoom(false);
    }

    public void SetPlayerName()
    {
        _playerName.Value = GetPlayerName();
    }

    private string GetPlayerName()
    {
        if (_playerNameInput.text != "")
            return _playerNameInput.text;
        else
            return $"Player{_roomData.Value.PlayerCount + 1}";
    }

    public override void OnJoinedRoom()
    {
        var customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        List<string> playerNames = (List<string>)PhotonNetwork.CurrentRoom.CustomProperties["PlayerNames"];

        if (playerNames == null)
            playerNames = new List<string>();

        if (playerNames.Contains(_playerName.Value))
        {
            //Player name alread taken - leave room
            LeaveRoom();
            return;
        }

        //Add player name to room properties
        playerNames.Add(_playerName.Value);
        customProperties["PlayerNames"] = playerNames;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);

        string message = $"Joined Room: {PhotonNetwork.CurrentRoom.Name}";
        Debug.Log(message);

        _UIMessageEvent.Raise(message);
        _inTeamLobby.Raise(true);
        _joinedRoomEvent.Raise();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        _UIMessageEvent.Raise(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        _UIMessageEvent.Raise(message);
    }
}