using Photon.Pun;
using ScriptableArchitecture.Data;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private StringReference _gameSceneName;


    public void StartGame()
    {
        if (_roomData.Value.PlayerCount == 0)
        {
            Debug.Log("Could not start game with 0 players");
            return;
        }

        PhotonNetwork.LoadLevel(_gameSceneName.Value);
    }
}