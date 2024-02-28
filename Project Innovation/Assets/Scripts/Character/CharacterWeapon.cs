using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterBase), typeof(Rigidbody))]
public class CharacterWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<WeaponPartDataReference> _currentParts = new List<WeaponPartDataReference>();

    private WeaponPartData _currentWeaponData;

    [Header("Settings")]
    [SerializeField] private bool _infiniteAmmo;
    private float _shootTimer;
    private int _shotCount;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Components")]
    private CharacterBase _characterBase;
    private Rigidbody _rigidbody;

    private void Start()
    {
        TryGetComponent(out _characterBase);
        TryGetComponent(out _rigidbody);
    }

    private void Update()
    {
        UpdateShooting();

        //Test
        CalculateWeapon();
    }

    private void UpdateShooting()
    {
        bool isShooting = _characterBase.ShootingInput;

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        if (isShooting && _shootTimer <= 0 && (_shotCount < _currentWeaponData.ShotCount || _infiniteAmmo))
        {
            _shootTimer = _currentWeaponData.FireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        _shotCount++;

        for (int i = 0; i < _currentWeaponData.BulletsPerShot; i++)
        {
            Bullet();
        }
    }

    private void Bullet()
    {
        Vector3 bulletDirection = transform.forward;
        float sprayAngle = Random.Range(-_currentWeaponData.Spread / 2, _currentWeaponData.Spread / 2);
        Quaternion sprayRotation = Quaternion.Euler(0, sprayAngle, 0);
        Vector3 sprayedDirection = (sprayRotation * bulletDirection).normalized;

        Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(_currentWeaponData, sprayedDirection, _characterBase.Team);

        ApplyRecoil(-sprayedDirection, _currentWeaponData.Recoil);
    }

    public void ApplyRecoil(Vector3 direction, float recoil)
    {
        _rigidbody.AddForce(direction * recoil, ForceMode.Impulse);
    }

    public void CalculateWeapon()
    {
        _currentWeaponData = new WeaponPartData();

        foreach(WeaponPartDataReference part in _currentParts)
            AddWeaponPartData(part.Value);

        if (HasBonus(out WeaponPartData bonus))
            AddWeaponPartData(bonus);
    }

    private void AddWeaponPartData(WeaponPartData part)
    {
        _currentWeaponData.DamagePerBullet += part.DamagePerBullet;
        _currentWeaponData.Spread += part.Spread;
        _currentWeaponData.BulletsPerShot += part.BulletsPerShot;
        _currentWeaponData.ShotCount += part.ShotCount;
        _currentWeaponData.FireRate += part.FireRate;
        _currentWeaponData.BulletSpeed += part.BulletSpeed;
        _currentWeaponData.Range += part.Range;
        _currentWeaponData.Recoil += part.Recoil;
        _currentWeaponData.EnemyRecoil += part.EnemyRecoil;
    }

    private bool HasWeaponType(WeaponPartType type)
    {
        if (_currentParts == null || _currentParts.Count == 0) return false;


        foreach (WeaponPartDataReference part in _currentParts)
        {
            if (part.Value.PartType == type) return true;
        }

        return false;
    }

    private bool TryGetWeaponPart(WeaponPartType type, out WeaponPartData part)
    {
        part = null;

        if (_currentParts == null || _currentParts.Count == 0) return false;

        for (int i = 0; i < _currentParts.Count; i++)
        {
            if (_currentParts[i].Value.PartType == type)
            {
                part = _currentParts[i].Value;
                return true;
            }
        }

        return false;
    }

    private bool HasBonus(out WeaponPartData bonus)
    {
        if (_currentParts == null && _currentParts.Count != 3) //Change this when more weapon parts
        {
            bonus = null;
            return false;
        }

        if (_currentParts.All(x => x.Value.Bonus == _currentParts.First().Value.Bonus))
        {
            bonus = _currentParts[0].Value.Bonus.Value;
            return true;
        }

        bonus = null;
        return false;
    }

}