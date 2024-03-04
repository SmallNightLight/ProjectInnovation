using ScriptableArchitecture.Data;
using UnityEngine;

public class EnterCharacterSelect : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BoolReference _enterEvent;
    [SerializeField] private RoomDataReference _roomData;


    public void Enter()
    {
        if (_roomData.Value.PlayerCount == 0)
        {
            Debug.Log("Could not start game with 0 players");
            return;
        }

        _enterEvent.Raise(true);
    }
}