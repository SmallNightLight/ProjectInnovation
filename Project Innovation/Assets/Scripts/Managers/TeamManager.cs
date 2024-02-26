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

    [Header("Components")]
    private PhotonView _photonView;


    public void Setup()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void AddPlayer()
    {
        _roomData.Value.TryAddPlayer(_playerName.Value);
        UpdateOtherRoomData();
    }

    public void UpdateOtherRoomData()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RemoveBufferedRPCs(_photonView.ViewID, nameof(UpdateRoomData));
            _photonView.RPC("UpdateRoomData", RpcTarget.OthersBuffered, _roomData.Value);
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void UpdateRoomData(RoomData roomData)
    {
        _roomData.Value = roomData;
    }
}