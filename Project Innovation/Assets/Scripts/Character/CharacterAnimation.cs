using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class CharacterAnimation : MonoBehaviour
{
    [Header("Data")]

    [Header("Settings")]
    [SerializeField] private FloatReference _characterSpeed;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    private CharacterBase _characterBase;
    private CharacterWeapon _characterWeapon;
    private Vector2 _oldDirection;

    private void Start()
    {
        TryGetComponent(out _characterBase);
        TryGetComponent(out _characterWeapon);
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector2 movementDirection = _characterBase.MovementInput.normalized;
        Vector2 facingDirection = _characterBase.DirectionInput.normalized;

        bool isWalking = movementDirection.magnitude > 0.01f;
        bool hasGun = _characterWeapon?.IsCarryingWeapon() ?? false;

        float angleInRadians = (Vector2.SignedAngle(facingDirection, movementDirection) + 90) * Mathf.Deg2Rad;
        Vector2 animationDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * _characterSpeed.Value;

        if (isWalking)
        {
            _animator.SetFloat("MovementHorizontal", -animationDirection.y, 0.1f, Time.deltaTime);
            _animator.SetFloat("MovementVertical", animationDirection.x, 0.1f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("MovementHorizontal", 0, 0.1f, Time.deltaTime);
            _animator.SetFloat("MovementVertical", 0, 0.1f, Time.deltaTime);
        }

        float turningAngle = Mathf.Clamp(Vector2.SignedAngle(facingDirection, _oldDirection), -1, 1);
        _animator.SetFloat("TurningAngle", turningAngle, 0.2f, Time.deltaTime);

        _oldDirection = facingDirection;
        _animator.SetBool("IsWalking", isWalking);
        _animator.SetBool("HasGun", hasGun);
    }
}