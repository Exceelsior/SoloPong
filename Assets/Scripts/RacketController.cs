using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]   
    private Rigidbody _rigidbody;
    [SerializeField]
    private Collider _rightCollider;
    [SerializeField]
    private float _moveSpeed = 10.0f;

    private Vector3 _touchPosition;
    private Vector3 _moveDirection;
    private bool _isPlayerDragging = false;
    private bool _playerStoppedDragging = false;

    #region Unity Messages
    void Update()
    {
        HandleInputs();
    }
    private void FixedUpdate()
    {
        if (_isPlayerDragging == false) return;
        HandleMovement();
    }

    #endregion

    private void HandleInputs()
    { 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _touchPosition = _camera.ScreenToWorldPoint(touch.position);
            _isPlayerDragging = true;

            _playerStoppedDragging = touch.phase == TouchPhase.Ended;
        }
        else _isPlayerDragging = false;
    }

    private void HandleMovement()
    {
        _moveDirection = _touchPosition - _rigidbody.transform.position;
        Debug.Log(_moveDirection);
        _rigidbody.velocity = _playerStoppedDragging ? Vector3.zero : new Vector3(_moveDirection.x, 0, _moveDirection.z) * _moveSpeed;
    }


}
