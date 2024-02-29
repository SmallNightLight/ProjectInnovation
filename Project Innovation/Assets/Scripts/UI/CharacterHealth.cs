using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterBase))]
public class CharacterHealth : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloatReference _startHealth;
    private float _health;

    [Header("Components")]
    private CharacterBase _characterBase;
    private Slider _healthSlider;

    private void Start()
    {
        TryGetComponent(out _characterBase);
        _healthSlider = GetComponentInChildren<Slider>();

        _health = _startHealth.Value;
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

    public void Death()
    {
        _health = 0;
        Debug.Log("Player died");
    }

    private void UpdateHealthUI()
    {
        _healthSlider.value = Mathf.Clamp(((100 / _startHealth.Value) * _health) / 100, 0, _startHealth.Value);
    }
}