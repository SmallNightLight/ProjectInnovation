using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class CharacterData : IDataPoint
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public float Health;
        public CharacterState CharacterState;
        public AnimationState AnimationState;
        public float AnimationTime;
        public List<WeaponType> Weapons;
    }

    [System.Serializable]
    public class Weapon
    {
        public int ID;
    }

    public enum WeaponType
    {
        None, Shovel, Gun
    }

    public enum CharacterState
    {
        NONE, CRAFTING, WEAPON
    }

    public enum AnimationState 
    { 
        IDLE, WALKING, DASHING, CRAFTING, 
    }

}