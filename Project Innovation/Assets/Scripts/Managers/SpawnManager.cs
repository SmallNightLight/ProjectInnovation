using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private PlayersInputReference _playersInput;

    [Header("Settings")]
    [SerializeField] private List<Vector3> _teamSpawnPositions;

    [Header("Prefabs")]
    [SerializeField] private GameObject _characterPrefab;

    public void Setup()
    {
        AddPlayers();
    }

    public void AddPlayers()
    {
        List<TeamData> teams = _roomData.Value.GetTeams();

        for (int team = 0; team < teams.Count; team++)
        {
            for (int player = 0; player < teams[team].Players.Count; player++)
            {
                SpawnPlayer(teams[team].Players[player], team);
            }
        }
    }

    public void SpawnPlayer(string playerName, int team)
    {
        if (_teamSpawnPositions == null || team >= _teamSpawnPositions.Count)
        {
            Debug.LogWarning("Could not spawn character - invalid spawn position");
            return;
        }

        _playersInput.Value.AddNewPlayer(playerName);
        CharacterBase character = Instantiate(_characterPrefab, _teamSpawnPositions[team], Quaternion.identity).GetComponent<CharacterBase>();
        character.InitializeCharacter(playerName, team);
    }
}