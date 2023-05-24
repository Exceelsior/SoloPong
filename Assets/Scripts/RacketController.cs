using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RacketController : MonoBehaviour
{
    public UnityAction ballHitEvent;


    [SerializeField]
    private Camera _camera;
    [SerializeField]   
    private Rigidbody _racketRigidbody;
    [SerializeField]
    private Collider _rightCollider;
    [SerializeField]
    private float _moveSpeed = 10.0f;
    [SerializeField]
    private Wall _wall;
    [SerializeField]
    private AnimationCurve _ballMaxTrajectoryHeightForHitSpeed;
    [SerializeField]
    private Vector3 _originalPosition;


    private Vector3 _pointOnPlane;
    private Vector3 _moveDirection;
    private bool _isPlayerDragging = false;
    private bool _playerStoppedDragging = false;
    private GameManager _gameManager;
    private Plane _plane;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        _plane = new Plane(Vector3.up, Vector3.up * transform.position.y);
    }

    public void HandleInputs()
    {
#if UNITY_EDITOR

        Vector3 mousePos= Input.mousePosition;
        
        Ray ray = _camera.ScreenPointToRay(mousePos);
        if(_plane.Raycast(ray, out float enter))
        {
            _pointOnPlane = ray.GetPoint(enter);
        }

        _isPlayerDragging = true;

        _playerStoppedDragging = false;
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = _camera.ScreenPointToRay(touch.position);

            if(_plane.Raycast(ray, out float enter))
            {
                _pointOnPlane = ray.GetPoint(enter);
            }

            _playerStoppedDragging = touch.phase == TouchPhase.Ended;
        }
        else _isPlayerDragging = false;

#endif
    }

    public void HandleMovement()
    {
        if (_isPlayerDragging == false) return;
        _moveDirection = _pointOnPlane - _racketRigidbody.transform.position;
        Debug.Log(_moveDirection);
        _racketRigidbody.velocity = _playerStoppedDragging ? Vector3.zero : new Vector3(_moveDirection.x, 0, _moveDirection.z) * _moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            ballHitEvent?.Invoke();

            Vector3 ballTarget = _wall.GetRandomPointOnBoundsFromBallPos(ball.transform.position);
            ball.ballRigidbody.velocity = GetBallVelocityToTarget(ball, ballTarget);


        }
    }

    private Vector3 GetBallVelocityToTarget(Ball ball, Vector3 targetBallPos)
    {
        Vector3 initialBallPos = ball.transform.position;
        float maxHeight = _ballMaxTrajectoryHeightForHitSpeed.Evaluate(_racketRigidbody.velocity.magnitude + ball.ballRigidbody.velocity.magnitude);
        float displacementY = targetBallPos.y - initialBallPos.y;
        Vector3 displacementXZ = new Vector3(targetBallPos.x - initialBallPos.x, 0, targetBallPos.z - initialBallPos.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(displacementY-2 * Physics.gravity.y * maxHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * maxHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - maxHeight) / Physics.gravity.y));
        return velocityXZ + velocityY;
    }

    public void ResetPosition()
    {
        _racketRigidbody.velocity = Vector3.zero;
        _racketRigidbody.MovePosition(_originalPosition);
    }

}
