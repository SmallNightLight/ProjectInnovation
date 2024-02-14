using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(Joystick))]
public class JoystickValue : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Vector2Reference _joyconInput;

    [Header("Components")]
    private Joystick _joystick;

    private void Start()
    {
        _joystick = GetComponent<Joystick>();
    }

    private void Update()
    {
        _joyconInput.Value = _joystick.Direction;
    }
}