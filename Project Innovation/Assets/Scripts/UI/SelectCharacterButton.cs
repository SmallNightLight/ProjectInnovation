using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataReference _characterData;
    [SerializeField] private CharacterDataReference _previewCharacterEvent;

    private void Start()
    {
        if (TryGetComponent(out Image image))
        {
            image.sprite = _characterData.Value.Icon;
        }
        else Debug.LogWarning("No Image found");
    }

    public void SelectCharacter()
    {
        _previewCharacterEvent.Raise(_characterData.Value);
    }
}