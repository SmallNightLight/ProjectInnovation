using ScriptableArchitecture.Data;
using System.Collections.Generic;
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

        int winTeam = 1;
        foreach(var v in _roomData.Value.GetTeams())
        {
            if (v.DeathCount >= _pointsForWin)
            {
                _over = true;
                List<string> pW = _roomData.Value.GetTeams()[winTeam].Players;
                _gameEndEvent.Raise(string.Join(",", pW)); //FAST FIX SRY
            }
            winTeam--;
        }
    }
}