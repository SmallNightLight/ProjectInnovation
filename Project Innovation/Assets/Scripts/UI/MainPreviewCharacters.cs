using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPreviewCharacters : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterCollectionReference _characterCollection;

    [SerializeField] private RoomDataReference _roomData;
    [SerializeField] private PlayerCharactersReference _playerCharacters;

    private Dictionary<string, GameObject> _references;

    [Header("Prefabs")]
    [SerializeField] private GameObject _characterPreviewPrefab;

    private void OnEnable()
    {
        SetupEmptyPreview();
        UpdateCharacterPreview();
    }

    public void SetupEmptyPreview()
    {
        _references = new Dictionary<string, GameObject>();

        foreach (string playerName in _roomData.Value.GetPlayerNames())
        {
            GameObject g = Instantiate(_characterPreviewPrefab, transform);
            _references[playerName] = g;
        }
    }

    public void UpdateCharacterPreview()
    {
        if (_references == null ||_references.Count == 0) return;

        foreach (var reference in _references)
        {
            string characterName = _playerCharacters.Value.GetCharacter(reference.Key);

            if (characterName == "") continue;

            if (_characterCollection.Value.TryGetCharacter(characterName, out CharacterData characterData))
                SetIcon(reference.Value, characterData.Icon);
            else Debug.LogWarning("Character name not found in collection");
        }
    }

    public void SetIcon(GameObject target, Sprite icon)
    {
        if (target.TryGetComponent(out Image image))
        {
            image.sprite = icon;
        }
        else Debug.LogWarning("Target does not have an image component");
    }
}