using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private PlayersInputReference _playersInput;

    [Header("Settings")]
    [SerializeField] private List<TeamSpawns> _teamSpawnPositions;

    [System.Serializable]
    private class TeamSpawns
    {
        public List<Vector3> PlayerSpawns;
    }

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
            teams[team].ResetDeathCounter();
            for (int player = 0; player < teams[team].Players.Count; player++)
            {
                SpawnPlayer(teams[team].Players[player], team, player);
            }
        }
    }

    public void SpawnPlayer(string playerName, int team, int playerIndex)
    {
        if (_teamSpawnPositions == null || team >= _teamSpawnPositions.Count)
        {
            Debug.LogWarning("Could not spawn character - invalid spawn position");
            return;
        }

        _playersInput.Value.AddNewPlayer(playerName);

        Vector3 spawnPosition = _teamSpawnPositions[team].PlayerSpawns[playerIndex];
        CharacterBase character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity).GetComponent<CharacterBase>();
        character.InitializeCharacter(playerName, team, spawnPosition);
    }
}