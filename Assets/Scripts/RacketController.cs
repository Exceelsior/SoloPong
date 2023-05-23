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

    private Vector3 _touchPosition;
    private Vector3 _moveDirection;
    private bool _isPlayerDragging = false;
    private bool _playerStoppedDragging = false;
    private GameManager _gameManager;


    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void HandleInputs()
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

    public void HandleMovement()
    {
        if (_isPlayerDragging == false) return;
        _moveDirection = _touchPosition - _racketRigidbody.transform.position;
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
            ball.ballRigidbody.velocity = GetBallVelocityToTarget(ball.transform.position, ballTarget);


        }
    }

    private Vector3 GetBallVelocityToTarget(Vector3 initialBallPos, Vector3 targetBallPos)
    {
        float maxHeight = _ballMaxTrajectoryHeightForHitSpeed.Evaluate(_racketRigidbody.velocity.magnitude);
        float displacementY = targetBallPos.y - initialBallPos.y;
        Vector3 displacementXZ = new Vector3(targetBallPos.x - initialBallPos.x, 0, targetBallPos.z - initialBallPos.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(displacementY-2 * Physics.gravity.y * maxHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * maxHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - maxHeight) / Physics.gravity.y));
        return velocityXZ + velocityY;
    }

}
