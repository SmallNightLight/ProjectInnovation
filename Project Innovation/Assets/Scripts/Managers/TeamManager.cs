using Photon.Pun;
using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TeamManager : MonoBehaviour, ISetupManager
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    
    [SerializeField] private StringReference _playerName;
    [SerializeField] private GameEvent _updateRoomDataVisuals;

    [Header("Loading game")]
    [SerializeField] private BoolReference _isMainGame;
    [SerializeField] private StringReference _mainSceneName;
    [SerializeField] private StringReference _controlerSceneName;

    [Header("Team selection")]
    [SerializeField] private BoolReference _enterCharacterSelection;

    [Header("Character selection")]
    [SerializeField] private PlayerCharactersReference _playerCharacters;
    [SerializeField] private PlayerCharactersReference _previewCharacters;
    [SerializeField] private GameEvent _previewChangeEvent;

    [Header("Components")]
    private PhotonView _photonView;


    public void Setup()
    {
        _photonView = GetComponent<PhotonView>();

        if (_isMainGame.Value)
        {
            _previewCharacters.Value.Clear();
            _playerCharacters.Value.Clear();
        }
    }

    public void AddPlayer()
    {
        if (_isMainGame.Value) return;

        _roomData.Value.TryAddPlayer(_playerName.Value);
        UpdateOtherRoomData();
        _updateRoomDataVisuals.Raise();
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
        }
        else
        {
            Debug.LogWarning("Photon not connected");
        }
    }

    [PunRPC]
    public void UpdateRoomData()
    {
        //Get room data from custom properties
        _roomData.Value = RoomData.CreateFromJson((string)PhotonNetwork.CurrentRoom.CustomProperties["Data"]);
        _updateRoomDataVisuals.Raise();
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

    //Enter character selection
    public void EnterCharacterSelect(bool isCalledFromThis)
    {
        if (!isCalledFromThis) return;

        _photonView.RPC("EnterCharacterSelectRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void EnterCharacterSelectRPC()
    {
        _enterCharacterSelection.Raise(false);
    }

    //Character preview
    public void PreviewCharacter(CharacterData characterData)
    {
        _photonView.RPC("SetPlayerPreview", RpcTarget.Others, _playerName.Value, characterData.Name);
    }

    [PunRPC]
    public void SetPlayerPreview(string playerName, string characterName)
    {
        if (!_isMainGame.Value) return;

        _previewCharacters.Value.SetCharacter(playerName, characterName);
        _previewChangeEvent.Raise();
    }


    //Character selection
    public void SelectCharacter()
    {
        _photonView.RPC("SetPlayerCharacter", RpcTarget.Others, _playerName.Value);
    }

    [PunRPC]
    public void SetPlayerCharacter(string playerName)
    {
        if (!_isMainGame.Value) return;

        _playerCharacters.Value.SetCharacter(playerName, _previewCharacters.Value.GetCharacter(playerName));

        if (AllCharactersSelected())
            StartGame();
    }

    public bool AllCharactersSelected() => _playerCharacters.Value.CharacterCount() == _roomData.Value.PlayerCount;
}