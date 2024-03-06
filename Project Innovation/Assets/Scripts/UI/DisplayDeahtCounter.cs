using ScriptableArchitecture.Data;
using TMPro;
using UnityEngine;

public class DisplayDeahtCounter : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private int _team;

    [Header("Settings")]
    [SerializeField] private string _textBefore;

    [Header("Components")]
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        UpdateDeathCounter();
    }

    private void UpdateDeathCounter()
    {
        TeamData teamData = _roomData.Value.GetTeamData(_team);
        if (teamData == null) return;

        _text.text = $"{_textBefore}{teamData.DeathCount}";
    }
}