using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterInput))]
public class CharacterWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData _testWeaponData;

    [Header("Settings")]
    private float _shootTimer;

    [Header("Prefabs")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Components")]
    private CharacterInput _characterInput;

    private void Start()
    {
        TryGetComponent(out _characterInput);
    }

    private void Update()
    {
        UpdateShooting();
    }

    private void UpdateShooting()
    {
        bool isShooting = _characterInput.ShootingInput;

        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        if (isShooting && _shootTimer <= 0)
        {
            _shootTimer = _testWeaponData.FireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);

        Vector3 bulletDirection = new Vector3(_characterInput.DirectionInput.x, 0, _characterInput.DirectionInput.y);
        bullet.GetComponent<Rigidbody>().AddForce(bulletDirection.normalized * _testWeaponData.BulletSpeed * 100);
    }
}