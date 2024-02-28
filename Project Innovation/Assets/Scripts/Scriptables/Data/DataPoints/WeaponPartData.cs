using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WeaponPartData : IDataPoint
    {
        [Header("Base")]
        public WeaponDataVariable MainWeapon;
        public WeaponPartType PartType;

        [Header("Percentages")]
        [Tooltip("% of the damage per individual bullet")] public float DamagePerBullet;
        [Tooltip("% of the amount of spread in degrees")] public float Spread;
        [Tooltip("% of the amount of bullets per shot")] public int BulletsPerShot;
        [Tooltip("% of the amount of shots")] public int ShotCount;
        [Tooltip("% of the time between the shots")] public float FireRate;
        [Tooltip("% of the speed of the bullets")] public float BulletSpeed;
        [Tooltip("% of the range of the shot / bullet")] public float Range;
        [Tooltip("% of the amount of recoil for the player after a shot")] public float Recoil;
        [Tooltip("% of the amount of recoil for the enemy after getting hit")] public float EnemyRecoil;

        [Header("Prefabs")]
        public GameObject PartPrefab;
        public GameObject ItemPrefab;
    }

    public enum WeaponPartType
    {
        Barrel,
        Base,
        Stock,
        Bonus
    }
}