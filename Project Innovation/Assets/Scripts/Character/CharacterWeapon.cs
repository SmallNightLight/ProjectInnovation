using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterBase), typeof(Rigidbody))]
public class CharacterWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<WeaponPartDataReference> _currentParts = new List<WeaponPartDataReference>();
    [SerializeField] private String2Reference _pickupPartEvent;
    [SerializeField] private StringReference _destroyWeaponEvent;
    [SerializeField] private StringReference _combineWeaponEvent;

    private WeaponData _currentWeaponData;

    [Header("Settings")]
    [SerializeField] private bool _infiniteAmmo;
    [SerializeField] private float _shakeMargin = 0.5f;
    private float _shootTimer;
    private int _shotCount;
    private List<Item> _pickupList = new List<Item>();
    private bool _isCombined;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Components")]
    private CharacterBase _characterBase;
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject _parentWeapon;
    private BulletExitPoint _bulletExitPoint;

    private void Start()
    {
        TryGetComponent(out _characterBase);
        TryGetComponent(out _rigidbody);

        CalculateWeapon();
    }

    private void Update()
    {
        CheckPickup();
        UpdateShooting();
    }

    private void CheckPickup()
    {
        if (_pickupList == null || _pickupList.Count == 0) return;

        if (!_characterBase.InteractingInput) return;

        for (int i = _pickupList.Count - 1; i >= 0; i--)
        {
            Item item = _pickupList[i];

            if (TryAddWeaponPart(item.WeaponPartData))
            {
                _pickupPartEvent.Raise(new String2 { Item1 = _characterBase.CharacterName, Item2 = item.WeaponPartData.Value.ID });
                _pickupList.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
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

        if (!_isCombined && CanCombine())
        {
            _combineWeaponEvent.Raise(_characterBase.CharacterName);
            _isCombined = true;
            UpdateWeaponVisuals();
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
        if (!HasWeaponType(WeaponPartType.Base) || _bulletExitPoint == null) return;

        Vector3 bulletDirection = transform.forward;
        float sprayAngle = Random.Range(-_currentWeaponData.Spread / 2, _currentWeaponData.Spread / 2);
        Quaternion sprayRotation = Quaternion.Euler(0, sprayAngle, 0);
        Vector3 sprayedDirection = (sprayRotation * bulletDirection).normalized;

        Bullet bullet = Instantiate(_bulletPrefab, _bulletExitPoint.transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(_currentWeaponData, sprayedDirection, _characterBase.Team);

        ApplyRecoil(-sprayedDirection, _currentWeaponData.Recoil);

        if (_shotCount >= _currentWeaponData.ShotCount && !_infiniteAmmo)
        {
            //No more bullets left - remove all parts
            RemoveCurrentWeapon();
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

        UpdateWeaponVisuals();
    }

    public void RemoveCurrentWeapon()
    {
        _currentParts.Clear();
        CalculateWeapon();
        _shotCount = 0;
        _isCombined = false;
        _destroyWeaponEvent.Raise(_characterBase.CharacterName);
    }

    public void UpdateWeaponVisuals()
    {
        foreach (Transform child in _parentWeapon.transform)
        {
            Destroy(child.gameObject);
        }

        if (_isCombined)
            AddWeaponObjects();
    }

    private void AddWeaponObjects()
    {
        _bulletExitPoint = null;

        List<(WeaponPartType, GameObject)> newParts = new List<(WeaponPartType, GameObject)>();

        foreach (WeaponPartDataReference part in _currentParts)
        {
            GameObject partObject = Instantiate(part.Value.PartPrefab, _parentWeapon.transform);
            newParts.Add((part.Value.PartType, partObject));

            if (part.Value.PartType == WeaponPartType.Barrel)
                _bulletExitPoint = partObject.GetComponentInChildren<BulletExitPoint>();
        }

        foreach (var part in newParts)
        {
            if (part.Item1 == WeaponPartType.Base)
                _bulletExitPoint = part.Item2.GetComponentInChildren<BulletExitPoint>();
        }
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
        if (other.gameObject.tag != "Item") return;

        if (other.gameObject.TryGetComponent(out Item item))
            _pickupList.Add(item);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Item") return;

        if (other.gameObject.TryGetComponent(out Item item))
            _pickupList.Remove(item);
    }

    private bool TryAddWeaponPart(WeaponPartDataReference part)
    {
        if (CanPickupItem(part))
        {
            _currentParts.Add(part);
            CalculateWeapon();
            return true;
        }

        return false;
    }

    private bool CanPickupItem(WeaponPartDataReference part) => !_isCombined && !HasWeaponType(part.Value.PartType);

    private bool CanCombine()
    {
        return _currentParts.Count != 0 && HasWeaponType(WeaponPartType.Base) && _characterBase.ShakeInput > _shakeMargin;
    }
}