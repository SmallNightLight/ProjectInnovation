using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class PlayerCharacters : IDataPoint
    {
        private Dictionary<string, string> _characters = new Dictionary<string, string>();
        private HashSet<string> _charactersReady = new HashSet<string>();

        public void Clear()
        {
            _characters = new Dictionary<string, string>();
            _charactersReady = new HashSet<string>();
        }

        public void SetCharacter(string playerName, string characterName)
        {
            _characters[playerName] = characterName;
        }

        public void RemoveCharacter(string playerName)
        {
            _characters.Remove(playerName);
        }

        public string GetCharacter(string playerName)
        {
            if (!_characters.ContainsKey(playerName)) return "";

            return _characters[playerName];
        }

        public void SetReady(string playerName)
        {
            _charactersReady.Add(playerName);
        }

        public Dictionary<string, string> GetAll() => _characters;

        public int CharacterCount() => _charactersReady.Count;
    }
}