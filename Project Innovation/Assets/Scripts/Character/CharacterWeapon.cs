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

        if (_shootTimer <= 0 && isShooting)
        {
            _shootTimer = 10;
            Shoot();
        }
    }

    private void Shoot()
    {

    }
}