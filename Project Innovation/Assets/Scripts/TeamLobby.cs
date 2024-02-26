using ScriptableArchitecture.Data;
using UnityEngine;

public class TeamLobby : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private StringReference _playerName;
    [SerializeField] private GameEvent _updateRoomDataEvent;

    [Header("Team data")]
    [SerializeField] private int _team;


    public void SetPlayerTeam()
    {
        _roomData.Value.SetTeam(_playerName.Value, _team);
        _updateRoomDataEvent.Raise();
    }
}