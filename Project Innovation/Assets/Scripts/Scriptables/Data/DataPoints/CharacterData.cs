using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class CharacterData : IDataPoint
    {
        public string Name;
        public string Description;
        public Sprite Icon;

        [Header("Prefabs")]
        public GameObject PreviewObject;
        public GameObject GameCharacter;
        public Sprite HealthIcon;
    }
}