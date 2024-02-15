using UnityEngine;
using Photon.Pun;
using TMPro;
using ScriptableArchitecture.Data;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Settings")]
    [SerializeField] private TMP_InputField _roomNameInput;

    [Header("Data")]
    [SerializeField] private BoolReference _createDefaultRoom;
    [SerializeField] private StringReference _gameSceneName;
    [SerializeField] private StringReference _defaultRoomName;


    private void Start()
    {
        if (_createDefaultRoom.Value)
        {
            CreateDefaultRoom();
        }
    }

    private void CreateDefaultRoom()
    {
        PhotonNetwork.CreateRoom(_defaultRoomName.Value);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_roomNameInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomNameInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel(_gameSceneName.Value);
    }
}