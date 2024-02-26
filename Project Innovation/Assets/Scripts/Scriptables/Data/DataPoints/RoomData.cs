using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class RoomData : IDataPoint
    {
        public List<Vector2> _teamStartPosition;
        public int PlayersPerTeam = 2;
        public int MaxTeams = 4;

        private List<TeamData> _teams = new();
        private int _playerCount = 0;

        public void SetTeam(string playerName, int team)
        {
            if (playerName == "" || team < 0 || team >= MaxTeams)
            {
                Debug.Log("Could not set player to team");
                return;
            }

            if (!HasPlayer(playerName))
            {
                //Add new player
                TryAddPlayer(playerName);
            }
            
            if (!HasPlayer(playerName, team))
            {
                //Add player to a different team

                //Remove player from current current team
                int oldTeam = 0;
                for (oldTeam = 0; oldTeam < _teams.Count; oldTeam++)
                {
                    if (_teams[oldTeam].Players.Contains(playerName))
                    {
                        _teams[oldTeam].Players.Remove(playerName);
                        break;
                    }
                }

                if (_teams[team].Players.Count < PlayersPerTeam)
                {
                    //team has an empty spot left
                    _teams[team].Players.Add(playerName);
                }
                else
                {
                    //team is full, switch players
                    int oldTeamIndex = _teams[oldTeam].Players.IndexOf(playerName);
                    string otherPlayerName = _teams[oldTeam].Players[oldTeamIndex];

                    _teams[oldTeam].Players[oldTeamIndex] = _teams[team].Players[PlayersPerTeam - 1];
                    _teams[team].Players[PlayersPerTeam - 1] = otherPlayerName;
                }
            }
        }

        public bool TryAddPlayer(string playerName)
        {
            if (HasPlayer(playerName)) return false;

            _playerCount++;

            if (_playerCount < PlayersPerTeam * _teams.Count)
            {
                //Find team with empty spot
                for (int i = 0; i < _teams.Count; i++)
                {
                    if (_teams[i].Players.Count < PlayersPerTeam)
                    {
                        _teams[i].Players.Add(playerName);
                        return true;
                    }
                }
            }

            //Add new team with new player
            _teams.Add(new TeamData());
            _teams[^1].Players.Add(playerName);
            return true;
        }

        public bool HasPlayer(string playerName)
        {
            for(int i = 0; i < _teams.Count; i++)
            {
                if (HasPlayer(playerName, i))
                    return true;
            }

            return false;
        }

        public bool HasPlayer(string playerName, int team)
        {
            for (int j = 0; j < _teams[team].Players.Count; j++)
            {
                if (_teams[team].Players[j] == playerName)
                    return true;
            }

            return false;
        }

        public bool CanAddPlayer(int team) => _teams[team].Players.Count < PlayersPerTeam;

        public List<TeamData> GetTeams() => _teams;

        public int PlayerCount => _playerCount;
    }
}