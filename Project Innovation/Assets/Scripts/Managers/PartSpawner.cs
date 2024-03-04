using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private List<GameObject> _spawnParts = new List<GameObject>();
    [SerializeField] private Vector2 _spawnTime;

    private void Start()
    {
        StartCoroutine(Spawner());
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
}