using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityAction ballFallEvent;
    public bool hasCollidedWithRacket;
    public bool hasCollidedWithWall;
    public Rigidbody ballRigidbody;
    [SerializeField]
    private Vector3 _originalPosition;
    [SerializeField]
    private AudioManager _audioManager;

    private GameManager _gameManager;

    

    private float _gameLoseHeight;
    public void Init(GameManager gameManager, float gameLoseHeight)
    {
        _gameManager = gameManager;
        _gameLoseHeight = gameLoseHeight;
    }

    public void HandleHeightDetection()
    {
        if(transform.position.y <= _gameLoseHeight) ballFallEvent?.Invoke();
    }

    public void ResetPosition()
    {
        hasCollidedWithWall = false;
        hasCollidedWithRacket = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.MovePosition(_originalPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioManager.PlayBallBounceSound(false);
    }

}
