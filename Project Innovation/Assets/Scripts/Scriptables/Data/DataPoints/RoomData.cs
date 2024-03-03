using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class RoomData : IDataPoint, IAssignment<RoomData>
    {
        public int PlayersPerTeam = 2;
        public int MaxTeams = 4;

        [SerializeField] private List<TeamData> _teams = new();
        [SerializeField] private int _playerCount = 0;

        public RoomData() 
        {
            PlayersPerTeam = 2;
            MaxTeams = 4;
            _teams = new List<TeamData>();
            _playerCount = 0;
        }

        public RoomData(int playersPerTeam, int maxTeams, List<TeamData> teams, int playerCount) 
        {
            PlayersPerTeam = playersPerTeam;
            MaxTeams = maxTeams;
            _teams = teams;
            _playerCount = playerCount;
        }

        public RoomData Copy()
        {
            List<TeamData> copiedTeams = new List<TeamData>();

            foreach (TeamData team in _teams)
            {
                TeamData copiedTeam = new TeamData();
                copiedTeam.Players = new List<string>(team.Players);
                copiedTeams.Add(copiedTeam);
            }

            return new RoomData(PlayersPerTeam, MaxTeams, copiedTeams, _playerCount);
        }

        //Serialization for photon
        public string Deserialize()
        {
            return JsonUtility.ToJson(this);
        }

        public static RoomData CreateFromJson(string data)
        {
            if (data.IsNullOrEmpty())
                return new RoomData(); //To chane data on room change the script parameters here

            return JsonUtility.FromJson<RoomData>(data);
        }

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
                    string otherPlayerName = _teams[team].Players[^1];

                    _teams[oldTeam].Players.Add(otherPlayerName);
                    _teams[team].Players[^1] = playerName;
                }
            }
        }

        public bool TryAddPlayer(string playerName)
        {
            if (HasPlayer(playerName)) return false;

            _playerCount++;

            if (_playerCount  <= PlayersPerTeam * _teams.Count)
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

        public void Reset()
        {
            _teams.Clear();
            _playerCount = 0;
        }

        public bool CanAddPlayer(int team) => _teams[team].Players.Count < PlayersPerTeam;

        public List<TeamData> GetTeams() => _teams;

        public TeamData GetTeamData(int team)
        {
            if (team >= TeamCount) return null;

            return _teams[team];
        }

        public List<string> GetPlayerNames()
        {
            List<string> names = new List<string>();

            foreach (TeamData team in _teams)
            {
                names.AddRange(team.Players);
            }

            return names;
        }

        public int PlayerCount => _playerCount;

        public int TeamCount
        {
            get
            {
                int teamCount = _playerCount / PlayersPerTeam;

                if (_playerCount % PlayersPerTeam != 0)
                {
                    teamCount++;
                }

                return teamCount;
            }
        }
    }
}