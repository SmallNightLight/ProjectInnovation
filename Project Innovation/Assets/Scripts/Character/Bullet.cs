using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody _rigidbody;

    [Header("ScriptData")]
    private Vector3 _startPosition;
    private WeaponData _weaponData;
    private int _team;

    public void InitializeBullet(WeaponData weaponData, Vector3 direction, int team)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(direction.normalized * weaponData.BulletSpeed * 100);

        _startPosition = transform.position;
        _weaponData = weaponData;
        _team = team;
    }

    private void Update()
    {
        if (Vector3.Distance(_startPosition, transform.position) >= _weaponData.Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (other.gameObject.TryGetComponent(out CharacterBase character) && character.Team != _team)
            {
                Debug.Log("HIT");
            }
        }
    }
}