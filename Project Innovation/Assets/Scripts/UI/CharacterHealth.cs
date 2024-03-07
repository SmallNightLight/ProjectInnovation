using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterBase))]
public class CharacterHealth : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloatReference _startHealth;
    [SerializeField] private RoomDataReference _roomData;
    private float _health;

    [Header("Settings")]
    [SerializeField] private Color _colorTeam1;
    [SerializeField] private Color _colorTeam2;

    [Header("Components")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _fillImage;
    private CharacterBase _characterBase;
    private Slider _healthSlider;

    private void Start()
    {
        TryGetComponent(out _characterBase);
        _healthSlider = GetComponentInChildren<Slider>();

        _health = _startHealth.Value;
    }

    public void Initialize(int team, CharacterData characterData)
    {
        if (team == 0)
            _fillImage.color = _colorTeam1;
        else
            _fillImage.color = _colorTeam2;

        _iconImage.sprite = characterData.HealthIcon;

    }

    private void Update()
    {
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        if (_health <= 0)
            Death();
    }

    [ContextMenu("DIE")]
    public void Death()
    {
        _roomData.Value.GetTeamData(_characterBase.Team).AddDeathPoint();
        _health = 0;
        Respawn();
    }

    public void Respawn()
    {
        _health = _startHealth.Value;
        transform.position = _characterBase.SpawnPosition;
    }

    private void UpdateHealthUI()
    {
        _healthSlider.value = Mathf.Clamp(((100 / _startHealth.Value) * _health) / 100, 0, _startHealth.Value);
    }
}