using ScriptableArchitecture.Data;
using UnityEngine;

public class ActivateSO : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _startState;

    [Header("Condition for enabling")]
    [SerializeField] private BoolReference _condition;
    [SerializeField] private bool _desiredValue;

    void Start()
    {
        if (_startState)
            EnableWithCondition();
        else
            Disable();
    }

    public void EnableWithCondition()
    {
        _target.SetActive(_condition.Value == _desiredValue);
    }

    public void Disable()
    {
        _target.SetActive(false);
    }
}