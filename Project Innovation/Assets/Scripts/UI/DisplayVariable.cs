using UnityEngine;
using TMPro;
using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;

/// <summary>
/// Displays the value of a variable with optional text before and after
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class DisplayVariable : MonoBehaviour
{
    [SerializeField] private Variable _variable;
    [SerializeField] private string _textBefore;
    [SerializeField] private string _textAfter;
    private TMP_Text _textAsset;

    /// <summary>
    /// Initializes ny getting the TMP_Text component
    /// </summary>
    private void Start()
    {
        _textAsset = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the displayed text with the value of the variable. Currently works with IntVariable, FloatVariable, BoolVariable, Vector2Variable and Vector3Variable
    /// </summary>
    private void Update()
    {
        if (_variable is StringVariable)
        {
            _textAsset.text = _textBefore + ((StringVariable)_variable).Value + _textAfter;
        }
        if (_variable is IntVariable)
        {
            _textAsset.text = _textBefore + ((IntVariable)_variable).Value.ToString() + _textAfter;
        }
        else if (_variable is FloatVariable)
        {
            _textAsset.text = _textBefore + ((FloatVariable)_variable).Value.ToString() + _textAfter;
        }
        else if (_variable is BoolVariable)
        {
            _textAsset.text = _textBefore + ((BoolVariable)_variable).Value.ToString() + _textAfter;
        }
        else if (_variable is Vector2Variable)
        {
            _textAsset.text = _textBefore + ((Vector2Variable)_variable).Value.ToString() + _textAfter;
        }
        else if (_variable is Vector3Variable)
        {
            _textAsset.text = _textBefore + ((Vector3Variable)_variable).Value.ToString() + _textAfter;
        }
    }
}