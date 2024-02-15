using ScriptableArchitecture.Data;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Vector2Reference _joyconInput;

    [Header("Settings")]
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _deaccceleration = 0.8f;
    [SerializeField] private float _maxSpeed = 50;

    [Header("Components")]
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;

    private void Start()
    {
        TryGetComponent(out _photonView);
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _meshRenderer);

        if (_photonView.IsMine)
        {
            Color newColor = AssignRandomColor();
            
        }
        else
        {

        }
    }

    private Color AssignRandomColor()
    {
        Material originalMaterial = _meshRenderer.material;
        Material newMaterial = new Material(originalMaterial);

        Color newColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        newMaterial.color = newColor;
        GetComponent<MeshRenderer>().material = newMaterial;

        return newColor;
    }

    [PunRPC]
    private void SyncColor(float r, float g, float b)
    {
        // Change the material color to the synchronized color
        if (_meshRenderer != null)
        {
            Material material = _meshRenderer.material;
            material.color = new Color(r, g, b);
            _meshRenderer.material = material;
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            Color materialColor = _meshRenderer.material.color;
            _photonView.RPC("SyncColor", RpcTarget.OthersBuffered, materialColor.r, materialColor.g, materialColor.b);
        }
    }

    private void FixedUpdate()
    {
        if (_photonView.IsMine)
        {
            SetMovement();
        }
    }

    private void SetMovement()
    {
        if (_rigidbody == null)
        {
            Debug.LogWarning("Cannot apply movement to character due to missing rigidbody");
            return;
        }

        Vector2 input = _joyconInput.Value.normalized;
        Vector3 inputVelocity = new Vector3(-input.y, 0, input.x);

        if (input.magnitude > 0)
        {
            _rigidbody.AddForce(_acceleration * inputVelocity, ForceMode.Acceleration);
        }
        else
        {
            _rigidbody.velocity *= _deaccceleration;
        }

        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }
}