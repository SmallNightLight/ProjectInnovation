using ScriptableArchitecture.Data;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private StringReference _gameEndEvent;
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private int _pointsForWin = 3;

    private bool _over;

    private void Start()
    {
        _over = false;
    }

    private void Update()
    {
        if (_over) return;

        foreach(var v in _roomData.Value.GetTeams())
        {
            if (v.DeathCount >= _pointsForWin)
            {
                _over = true;
                _gameEndEvent.Raise(JsonUtility.ToJson(v.Players));
            }
        }
    }
}