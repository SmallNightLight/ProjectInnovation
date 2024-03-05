using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Data")]
    public WeaponPartDataReference WeaponPartData;

    [Header("Settings")]
    [SerializeField] private Color _hoverEmmissionColor;

    [Header("Components")]
    private Renderer _renderer;

    private List<CharacterBase> _touchingCharacters = new List<CharacterBase>();

    private Material _mainMaterial;
    private Material _hoverMaterial;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer == null) return;

        _mainMaterial = _renderer.material;

        _hoverMaterial = new Material(_mainMaterial);
        _hoverMaterial.SetColor("_EmissionColor", _hoverEmmissionColor);
    }

    private void Update()
    {
        if (_renderer == null || _touchingCharacters == null) return;

        _renderer.material = _touchingCharacters.Count == 0 ? _mainMaterial : _hoverMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Character") return;

        if (other.gameObject.TryGetComponent(out CharacterBase character))
            _touchingCharacters.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Character") return;

        if (other.gameObject.TryGetComponent(out CharacterBase character))
            _touchingCharacters.Remove(character);
    }
}