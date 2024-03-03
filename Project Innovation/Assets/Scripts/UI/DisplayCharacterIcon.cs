using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharacterIcon : MonoBehaviour
{
    [SerializeField] private CharacterDataReference _characterData;

    private void OnEnable()
    {
        if (TryGetComponent(out Image image))
        {
            image.sprite = _characterData.Value.Icon;
        }
    }
}