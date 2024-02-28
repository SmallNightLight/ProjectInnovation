using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WeaponData : IDataPoint
    {
        [Header("Stats")]
        [Tooltip("The damage per individual bullet")] public float DamagePerBullet;
        [Tooltip("The amount of spread in degrees")] public float Spread;
        [Tooltip("The amount of bullets per shot")] public int BulletsPerShot;
        [Tooltip("The amount of shots")] public int ShotCount;
        [Tooltip("The time between the shots")] public float FireRate;
        [Tooltip("The speed of the bullets")] public float BulletSpeed;
        [Tooltip("The range of the shot / bullet")] public float Range;
        [Tooltip("The amount of recoil for the player after a shot")] public float Recoil;
        [Tooltip("The amount of recoil for the enemy after getting hit")] public float EnemyRecoil;

        [Tooltip("The bonus for when the parts with the same bonus are together. Don't set it when PartType is Bonus")] public WeaponPartDataVariable Bonus;
    }
}