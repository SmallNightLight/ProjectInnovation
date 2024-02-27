using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TeamManager : MonoBehaviour, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private StringReference _playerName;
    [SerializeField] private GameEvent _updateRoomData;

    [Header("Loading game")]
    [SerializeField] private BoolReference _isMainGame;
    [SerializeField] private StringReference _mainSceneName;
    [SerializeField] private StringReference _controlerSceneName;

    [Header("Components")]
    private PhotonView _photonView;


    public void Setup()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void AddPlayer()
    {
        if (_isMainGame.Value) return;

        _roomData.Value.TryAddPlayer(_playerName.Value);
        UpdateOtherRoomData();
        _updateRoomData.Raise();
    }

    public void UpdateOtherRoomData()
    {
        Debug.Log("Update Room Data");

        if (PhotonNetwork.IsConnected)
        {
            //Using custom properties
            var customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customProperties["Data"] = _roomData.Value.Deserialize();
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
            _photonView.RPC("UpdateRoomData", RpcTarget.Others);

            //PhotonNetwork.RemoveBufferedRPCs(_photonView.ViewID, nameof(UpdateRoomData));
            //_photonView.RPC("UpdateRoomData", RpcTarget.OthersBuffered, _roomData.Value.Deserialize());
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void UpdateRoomData() //string roomData
    {
        //_roomData.Value = RoomData.CreateFromJson(roomData);

        //Get room data from custom properties
        _roomData.Value = RoomData.CreateFromJson((string)PhotonNetwork.CurrentRoom.CustomProperties["Data"]);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsConnected)
        {
            _photonView.RPC("LoadNextGameScene", RpcTarget.All);
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void LoadNextGameScene()
    {
        PhotonNetwork.LoadLevel(_isMainGame.Value ? _mainSceneName.Value : _controlerSceneName.Value);
    }
}