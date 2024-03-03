using UnityEngine;
using Photon.Pun;
using TMPro;
using ScriptableArchitecture.Data;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private StringReference _roomName;

    [SerializeField] private BoolReference _createDefaultRoom;
    [SerializeField] private StringReference _defaultRoomName;

    [SerializeField] private StringReference _playerName;
    [SerializeField] private BoolReference _inTeamLobby;
    [SerializeField] private StringReference _UIMessageEvent;
    [SerializeField] private GameEvent _joinedRoomEvent;

    [Header("Settings")]
    [SerializeField] private BoolReference _isMainGame;

    [Header("Components")]
    [SerializeField] private TMP_InputField _createRoomNameInput;

    [Space]

    [SerializeField] private TMP_InputField _joinRoomNameInput;
    [SerializeField] private TMP_InputField _joinPlayerNameInput;

    
    public void Setup()
    {
        if (_createDefaultRoom.Value)
        {
            CreateDefaultRoom();
        }
    }

    private void CreateDefaultRoom()
    {
        PhotonNetwork.CreateRoom(_defaultRoomName.Value);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_createRoomNameInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_joinRoomNameInput.text);
    }

    public void LeaveRoom()
    {
        Log($"Room already has a player with the name: {_playerName.Value}");
        PhotonNetwork.LeaveRoom(false);
    }

    public void SetPlayerName()
    {
        _playerName.Value = GetPlayerName();
    }

    private string GetPlayerName()
    {
        if (_isMainGame.Value)
            return "";

        if (_joinPlayerNameInput.text != "")
            return _joinPlayerNameInput.text;

        return $"Player{_roomData.Value.PlayerCount + 1}";
    }

    public override void OnJoinedRoom()
    {
        _roomName.Value = PhotonNetwork.CurrentRoom.Name;

        if (_isMainGame.Value)
        {
            Debug.Log("Joined room as Main");
            SetPlayerName();
        }
        else
        {
            //Get room data
            _roomData.Value = RoomData.CreateFromJson((string)PhotonNetwork.CurrentRoom.CustomProperties["Data"]);
            SetPlayerName();

            List<TeamData> teams = _roomData.Value.GetTeams();

            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].Players.Contains(_playerName.Value))
                {
                    //Player name alread taken - leave room
                    LeaveRoom();
                    return;
                }
            }
        }

        Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");

        _inTeamLobby.Raise(true);
        _joinedRoomEvent.Raise();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Log(message);
    }

    public void Log(string message)
    {
        Debug.Log(message);
        _UIMessageEvent.Raise(message);
    }
}