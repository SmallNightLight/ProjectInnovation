using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class CharacterCollection : IDataPoint
    {
        public List<CharacterDataVariable> Characters;

        public bool TryGetCharacter(string characterName, out CharacterData characterData)
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].Value.Name == characterName)
                {
                    characterData = Characters[i].Value;
                    return true;
                }
            }

            characterData = null;
            return false;
        }
    }
}