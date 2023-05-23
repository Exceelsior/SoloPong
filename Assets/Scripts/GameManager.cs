using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private RacketController _playerRacket;
    [SerializeField]
    private Wall _wall;
    [SerializeField]
    private Ball _ball;

    [SerializeField]
    private float _ballLoseHeight = -0.5f;

    public int TotalHits { get => _totalHits; }

    private int _totalHits;
    private bool _gameLose;

    private void Start()
    {
        _wall.Init(this);
        _ball.Init(this, _ballLoseHeight);
        _ball.ballFallEvent += GameLose;
        _playerRacket.Init(this);
        _playerRacket.ballHitEvent += () => _totalHits++;

    }

    private void Update()
    {
        if (_gameLose) return;
        _playerRacket.HandleInputs();
        _ball.HandleHeightDetection();
    }

    private void FixedUpdate()
    {
        _playerRacket.HandleMovement();
    }

    private void GameLose()
    {
        _ball.ResetPosition();
    }


}
