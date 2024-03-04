using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private VideoPlayer video;
    [SerializeField] GameObject text; 

    bool _hasJoined, videoEnded;

    private void Start()
    {
        video.loopPointReached += End;

        Debug.Log("Trying to connect...");
        PhotonNetwork.ConnectUsingSettings();

        StartCoroutine(WaitForVideo());
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        _hasJoined = true;
    }

    public void End(VideoPlayer p) 
    { 
        videoEnded = true;
        p.gameObject.SetActive(false);
        text.SetActive(true);
    }

    public IEnumerator WaitForVideo()
    {
        while (true)
        {
            if (_hasJoined && videoEnded) break;

            yield return null;
        }

        SceneManager.LoadScene(_nextSceneName);
    }
}