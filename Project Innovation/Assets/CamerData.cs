using ScriptableArchitecture.Data;
using UnityEngine;

public class CamerData : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Vector3Reference _cameraPosition;
    [SerializeField] private QuaternionReference _cameraRotation;

    private void Update()
    {
        _cameraPosition.Value = transform.position;
        _cameraRotation.Value = transform.rotation;
    }
}