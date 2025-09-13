using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private int _jumpHeight = 3;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private UnityEvent _onJump;
    [SerializeField] private UnityEvent<bool> _onCrouch;

    private Rigidbody _rb;
    public float Horizontal {get; private set;}
    public float Vertical {get; private set;}
    public bool IsRunning { get; private set;}
    public bool IsCrouching { get; private set;}
    private Vector3 _direction;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private bool _canMove = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void Update()
    {
        if (_cameraTransform == null) return;

        // Movement
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        _camForward = _cameraTransform.forward.normalized;
        _camRight = _cameraTransform.right.normalized;
        _camForward.y = 0f;
        _camRight.y = 0f;

        _direction = (Vertical * _camForward + Horizontal * _camRight).normalized * _speed;

        if (_direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            _rb.AddForce(Vector3.up * Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            _onJump?.Invoke();
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && Grounded()) 
        {
            _speed = _walkSpeed * 2;
            IsRunning = true;
        }
        else
        {
            _speed = _walkSpeed;
            IsRunning = false;
        }

        // Bow
        if (Input.GetKey(KeyCode.LeftControl) && Grounded())
        {
            IsCrouching = true;
            _onCrouch?.Invoke(IsCrouching);
            _speed = 0f;
        }
        else
        {
            IsCrouching = false;
            _onCrouch?.Invoke(IsCrouching);
        }

    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            _rb.velocity = _direction + new Vector3(0f, _rb.velocity.y, 0f);
        }
    }

    private bool Grounded()
    {
        if (_groundChecker.IsGrounded) return true;
        return false;
    }

    public IEnumerator TemporarilyDisableMovement()
    {
        _canMove = false;
        yield return new WaitForSeconds(1f); 
        _canMove = true;
    }
}
