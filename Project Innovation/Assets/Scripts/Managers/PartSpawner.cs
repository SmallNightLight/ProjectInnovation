using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private List<GameObject> _spawnParts = new List<GameObject>();
    [SerializeField] private List<GameObject> _movingSpawnParts= new List<GameObject>();
    [SerializeField] private Vector2 _spawnTime;
    [SerializeField] private Vector2 _frequency;
    [SerializeField] private float _speed;
    [SerializeField] private PhysicMaterial _m;

    private void Start()
    {
        StartCoroutine(Spawner());

        foreach(GameObject g in _movingSpawnParts) StartCoroutine(MovingSpawner(g));
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            SpawnPart();
            yield return null;
            yield return new WaitForSeconds(Random.Range(_spawnTime.x, _spawnTime.y));
        }
    }

    public void SpawnPart()
    {
        Transform spawnPoint = GetspawnPoint();

        if (spawnPoint == null) return;

        GameObject part = _spawnParts[Random.Range(0, _spawnParts.Count - 1)];
        Instantiate(part, spawnPoint);
    }

    private Transform GetspawnPoint()
    {
        List<Transform> availableSpawnPositions = new List<Transform>();
        foreach (Transform spawn in _spawnPositions)
        {
            if (spawn.childCount == 0)
                availableSpawnPositions.Add(spawn);
        }

        if (availableSpawnPositions.Count > 0)
            return availableSpawnPositions[Random.Range(0, availableSpawnPositions.Count - 1)];

        return null;
    }

    IEnumerator MovingSpawner(GameObject target)
    {
        while (true)
        {
            SpawnMovingPart(target.transform);
            yield return null;
            yield return new WaitForSeconds(Random.Range(_frequency.x, _frequency.y));
        }
    }

    public void SpawnMovingPart(Transform spawnPoint)
    {
        if (spawnPoint == null) return;

        GameObject part = _spawnParts[Random.Range(0, _spawnParts.Count - 1)];
        GameObject g = Instantiate(part, spawnPoint);
        SphereCollider m = g.AddComponent<SphereCollider>();
        m.material = _m;

        var rb = g.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        var v = g.AddComponent<ItemMoving>();
        Vector3 force = spawnPoint.forward * _speed;
        force = new Vector3(force.x, -3, force.z);
        v.Force = force;

        Destroy(g, 20);
    }
}