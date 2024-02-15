using Photon.Pun;
using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, ISetupManager
{
    [Header("Settings")]
    [SerializeField] private bool _connectAutomatic;
    [SerializeField] private string _loadingScene;

    [Header("Data")]
    [SerializeField] private BoolReference _createDefaultRoom;
    [SerializeField] private StringReference _gameSceneName;


    void ISetupManager.Setup()
    {
        if (_connectAutomatic && (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom))
        {
            LoadDefaultRoom();
        }
    }

    private void LoadDefaultRoom()
    {
        _createDefaultRoom.Value = true;
        _gameSceneName.Value = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_loadingScene);
    }
}