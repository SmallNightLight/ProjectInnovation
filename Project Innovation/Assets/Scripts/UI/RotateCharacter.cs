using ScriptableArchitecture.Data;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    [SerializeField] private Vector3 _axis = new Vector3(1, 0, 0);
    [SerializeField] private float _speed = 10f;
    [SerializeField] private BoolReference _inPreview;

    private void Update()
    {
        if (_inPreview.Value)
            transform.Rotate(_axis.normalized * _speed * Time.deltaTime);
    }
}