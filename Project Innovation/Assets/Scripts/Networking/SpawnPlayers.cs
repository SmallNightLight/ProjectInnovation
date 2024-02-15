using UnityEngine;
using Photon.Pun;


public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private Vector3 _startPosition;


    private void Start()
    {
        PhotonNetwork.Instantiate(_characterPrefab.name, _startPosition, Quaternion.identity);
    }
}