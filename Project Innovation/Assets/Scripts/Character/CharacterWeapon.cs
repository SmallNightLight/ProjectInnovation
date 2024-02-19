using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class CharacterWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData _testWeaponData;

    [Header("Settings")]
    private float _shootTimer;
    private int _shotCount;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Components")]
    private CharacterBase _characterBase;

    private void Start()
    {
        TryGetComponent(out _characterBase);
    }

    private void Update()
    {
        UpdateShooting();
    }

    private void UpdateShooting()
    {
        bool isShooting = _characterBase.ShootingInput;

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        if (isShooting && _shootTimer <= 0 && _shotCount < _testWeaponData.ShotCount)
        {
            _shootTimer = _testWeaponData.FireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        _shotCount++;

        for (int i = 0; i < _testWeaponData.BulletsPerShot; i++)
        {
            Bullet();
        }
    }

    private void Bullet()
    {
        Vector3 bulletDirection = new Vector3(_characterBase.DirectionInput.x, 0, _characterBase.DirectionInput.y);
        float sprayAngle = Random.Range(-_testWeaponData.Spread / 2, _testWeaponData.Spread / 2);
        Quaternion sprayRotation = Quaternion.Euler(0, sprayAngle, 0);
        Vector3 sprayedDirection = sprayRotation * bulletDirection;

        Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(_testWeaponData, sprayedDirection, _characterBase.Team);
    }
}