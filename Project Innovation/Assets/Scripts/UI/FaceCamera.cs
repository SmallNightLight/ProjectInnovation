using ScriptableArchitecture.Data;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private QuaternionReference _cameraRotation;

    private void Update()
    {
        transform.LookAt(transform.position + _cameraRotation.Value * Vector3.forward, _cameraRotation.Value * Vector3.up);
    }
}