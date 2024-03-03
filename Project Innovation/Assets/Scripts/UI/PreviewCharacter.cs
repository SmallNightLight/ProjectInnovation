using ScriptableArchitecture.Data;
using UnityEngine;

public class PreviewCharacter : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataReference _characterData;
    [SerializeField] private BoolReference _hasSelectedCharacter;

    [Header("Components")]
    [SerializeField] private GameObject _target;

    private void Start()
    {
        _target.SetActive(false);
    }

    public void Preview(CharacterData characterData)
    {
        bool activate = characterData == _characterData.Value;

        _target.SetActive(activate);

        if (activate)
            _hasSelectedCharacter.Raise(true);
    }
}