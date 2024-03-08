using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class CharacterAnimation : MonoBehaviour
{
    [Header("Components")]
    private Animator _animator;
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

        Animator animator = Animator;

        if (isWalking)
        {
            facingDirection = new Vector2(facingDirection.y, -facingDirection.x);

            if (facingDirection == Vector2.zero)
                facingDirection = _oldDirection;

            float angleInRadians = Vector2.SignedAngle(facingDirection, movementDirection) * Mathf.Deg2Rad;
            Vector2 animationDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            animator.SetFloat("MovementHorizontal", animationDirection.y, 0.1f, Time.deltaTime);
            animator.SetFloat("MovementVertical", animationDirection.x, 0.1f, Time.deltaTime);

            _oldDirection = facingDirection;
        }
        else
        {
            animator.SetFloat("MovementHorizontal", 0, 0.1f, Time.deltaTime);
            animator.SetFloat("MovementVertical", 0, 0.1f, Time.deltaTime);
        }

        float turningAngle = Mathf.Clamp(Vector2.SignedAngle(facingDirection, _oldDirection), -1, 1);
        animator.SetFloat("TurningAngle", turningAngle, 0.2f, Time.deltaTime);

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("HasGun", hasGun);
    }

    public Animator Animator 
    { 
        get 
        { 
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
                
            return _animator; 
        } 
    }
}