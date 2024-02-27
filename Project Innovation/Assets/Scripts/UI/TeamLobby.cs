using ScriptableArchitecture.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamLobby : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BoolReference _isMainGame;
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private StringReference _playerName;
    [SerializeField] private GameEvent _updateOtherRoomData;
    [SerializeField] private GameEvent _updateRoomDataVisuals;

    [Header("Team data")]
    [SerializeField] private int _team;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerNamePrefab;

    [Header("References")]
    [SerializeField] private GameObject _playersParent;
    [SerializeField] private List<GameObject> _visualChildren;


    public void Update()
    {
        SetVisibility();
    }

    private void SetVisibility()
    {
        if (_visualChildren == null) return;

        bool active = !_isMainGame.Value && _team < _roomData.Value.TeamCount;

        foreach (GameObject child in _visualChildren)
        {
            child.SetActive(active);
        }
    }

    public void SetPlayerTeam()
    {
        _roomData.Value.SetTeam(_playerName.Value, _team);
        _updateOtherRoomData.Raise();
        _updateRoomDataVisuals.Raise();
    }

    public void UpdatePlayers()
    {
        //Remove players
        foreach (Transform child in _playersParent.transform)
        {
            Destroy(child.gameObject);
        }

        var teamData = _roomData.Value.GetTeamData(_team);

        if (teamData == null) return;

        //Add players
        foreach (string playerName in teamData.Players)
        {
            if (Instantiate(_playerNamePrefab, _playersParent.transform).TryGetComponent(out TMP_Text text))
                text.text = playerName;
            else
                Debug.Log("Did not find TMP component");
        }
    }
}