using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterBase), typeof(Rigidbody))]
public class CharacterWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<WeaponPartDataReference> _currentParts = new List<WeaponPartDataReference>();

    private WeaponData _currentWeaponData;

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
        //Only shoot when has a base part
        if (!HasWeaponType(WeaponPartType.Base)) return;

        Vector3 bulletDirection = transform.forward;
        float sprayAngle = Random.Range(-_currentWeaponData.Spread / 2, _currentWeaponData.Spread / 2);
        Quaternion sprayRotation = Quaternion.Euler(0, sprayAngle, 0);
        Vector3 sprayedDirection = (sprayRotation * bulletDirection).normalized;

        Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(_currentWeaponData, sprayedDirection, _characterBase.Team);

        ApplyRecoil(-sprayedDirection, _currentWeaponData.Recoil);

        if (_shotCount >= _currentWeaponData.ShotCount && !_infiniteAmmo)
        {
            //No more bullets left - remove all parts
            _currentParts.Clear();
            CalculateWeapon();
        }
    }

    public void ApplyRecoil(Vector3 direction, float recoil)
    {
        _rigidbody.AddForce(direction * recoil, ForceMode.Impulse);
    }

    [ContextMenu("Calculate weapon")]
    public void CalculateWeapon()
    {
        _currentWeaponData = new WeaponData();

        foreach(WeaponPartDataReference part in _currentParts)
            AddWeaponPartData(part.Value);

        if (HasBonus(out WeaponPartData bonus))
            AddWeaponPartData(bonus);
    }

    private void AddWeaponPartData(WeaponPartData part)
    {
        WeaponData mainWeapon = part.MainWeapon.Value;

        _currentWeaponData.DamagePerBullet += mainWeapon.DamagePerBullet * part.DamagePerBullet / 100;
        _currentWeaponData.Spread += mainWeapon.Spread * part.Spread / 100;
        _currentWeaponData.BulletsPerShot += mainWeapon.BulletsPerShot * part.BulletsPerShot / 100;
        _currentWeaponData.ShotCount += mainWeapon.ShotCount * part.ShotCount / 100;
        _currentWeaponData.FireRate += mainWeapon.FireRate * part.FireRate / 100;
        _currentWeaponData.BulletSpeed += mainWeapon.BulletSpeed * part.BulletSpeed / 100;
        _currentWeaponData.Range += mainWeapon.Range * part.Range / 100;
        _currentWeaponData.Recoil += mainWeapon.Recoil * part.Recoil / 100;
        _currentWeaponData.EnemyRecoil += mainWeapon.EnemyRecoil * part.EnemyRecoil / 100;
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
        if (_currentParts == null || _currentParts.Count != 3) //Change this when more weapon parts
        {
            bonus = null;
            return false;
        }

        if (_currentParts.All(x => x.Value.MainWeapon == _currentParts.First().Value.MainWeapon))
        {
            bonus = _currentParts[0].Value.MainWeapon.Value.Bonus.Value;
            return true;
        }

        bonus = null;
        return false;
    }

    //Weapon part pickup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            if (other.gameObject.TryGetComponent(out Item item))
            {
                AddWeaponPart(item.WeaponPartData);
                Destroy(other.gameObject);
            }
        }
    }

    private void AddWeaponPart(WeaponPartDataReference part)
    {
        if (!HasWeaponType(part.Value.PartType))
        {
            _currentParts.Add(part);
            CalculateWeapon();
        }
    }
}