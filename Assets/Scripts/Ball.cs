using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityAction ballFallEvent;

    public Rigidbody ballRigidbody;
    private GameManager _gameManager;

    private float _gameLoseHeight;
    private Vector3 _originalPosition;
    public void Init(GameManager gameManager, float gameLoseHeight)
    {
        _gameManager = gameManager;
        _gameLoseHeight = gameLoseHeight;
        _originalPosition = transform.position;
    }

    public void HandleHeightDetection()
    {
        if(transform.position.y <= _gameLoseHeight) ballFallEvent?.Invoke();
    }

    public void ResetPosition()
    {
        ballRigidbody.velocity = Vector3.zero;
        transform.position = _originalPosition;
    }
}
