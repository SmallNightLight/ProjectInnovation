using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class PlayersInput : IDataPoint
    {
        private Dictionary<string, PlayerInput> _data = new Dictionary<string, PlayerInput>();

        public void AddNewPlayer(string playerName)
        {
            _data.Add(playerName, new PlayerInput());
        }

        public void RemovePlayer(string playerName) 
        {
            _data.Remove(playerName);
        }

        public bool TryGetPlayerInput(string playerName, out PlayerInput playerInput)
        {
            if (HasPlayer(playerName))
            {
                playerInput = _data[playerName];
                return true;
            }

            playerInput = null;
            return false;
        }

        public bool TrySetPlayerInput(string playerName, Vector2 movementInput, Vector2 directionInput, bool interactingInput, bool shootingInput)
        {
            if (HasPlayer(playerName))
            {
                _data[playerName].MovementInput = movementInput;
                _data[playerName].DirectionInput = directionInput;
                _data[playerName].InteractingInput = interactingInput;
                _data[playerName].ShootingInput = shootingInput;

                return true;
            }

            return false;
        }

        public bool HasPlayer(string playerName) => _data.ContainsKey(playerName);
    }

    [System.Serializable]
    public class PlayerInput : IDataPoint 
    {
        public Vector2 MovementInput;
        public Vector2 DirectionInput;
        public bool InteractingInput;
        public bool ShootingInput;
    }
}