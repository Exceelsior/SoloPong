using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private RacketController _playerRacket;
    [SerializeField]
    private Wall _wall;
    [SerializeField]
    private Ball _ball;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _moneyText;
    [SerializeField]
    private GameObject _endGameUI;
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private TextMeshProUGUI _highscoreText;
    [SerializeField]
    private TextMeshProUGUI _newHighScoreText;

    [SerializeField]
    private float _moneyGainAnimationDuration = 1.0f;
    [SerializeField]
    private AnimationCurve _scoreGainScaleAnimationCurve;


    [SerializeField]
    private float _ballLoseHeight = -0.5f;

    public int TotalHits { get => _totalHits; }

    private int _totalHits;
    private bool _gameLose;
    private bool _gamePaused;
    private int _basePlayerMoney;
    private int _newPlayerMoney;
    private Coroutine _scoreGainCoroutine;
    private int _highscore;


    private float _scoreGainAnimationDuration;

    #region Unity Events
    private void Start()
    { 
        _scoreGainAnimationDuration = _scoreGainScaleAnimationCurve.keys[_scoreGainScaleAnimationCurve.keys.Length - 1].time;
        _wall.Init(this);
        _ball.Init(this, _ballLoseHeight);
        _ball.ballFallEvent += GameLose;
        _playerRacket.Init(this);
        _playerRacket.ballHitEvent += IncrementScore;

        SetupGame();
    }

    private void Update()
    {
        if (_gameLose || _gamePaused) return;
        _playerRacket.HandleInputs();
        _ball.HandleHeightDetection();
    }

    private void FixedUpdate()
    {
        if (_gameLose || _gamePaused) return;
        _playerRacket.HandleMovement();
    }
    #endregion
    public void PauseGameSwitch()
    {
        _gamePaused = !_gamePaused;
    }
    private void GameLose()
    {
        _gameLose = true;
        _newPlayerMoney = _basePlayerMoney + _totalHits;
        PlayerPrefs.SetInt("PlayerMoney", _newPlayerMoney);
        _endGameUI.SetActive(true);
        _scoreText.gameObject.SetActive(false);


        if (_totalHits > _highscore)
        {
            PlayerPrefs.SetInt("Highscore", _totalHits);
            _newHighScoreText.gameObject.SetActive(true);
        }

        StartCoroutine(MoneyGainCoroutine());
    }

    private void IncrementScore()
    {
        if(_scoreGainCoroutine != null)
        {
            StopCoroutine(_scoreGainCoroutine);
        }

        _totalHits++;
        _scoreText.text = _totalHits.ToString() + "\nHits";

        StartCoroutine(ScoreGainCoroutine());
    }

    private IEnumerator ScoreGainCoroutine() 
    {
        for (float i = 0.0f; i < _scoreGainAnimationDuration; i += Time.deltaTime)
        {
            _scoreText.rectTransform.localScale = Vector3.one * _scoreGainScaleAnimationCurve.Evaluate(i);
            yield return null;
        }

        _scoreGainCoroutine = null;
    }

    private IEnumerator MoneyGainCoroutine()
    {
        for(float i = 0.0f; i < _moneyGainAnimationDuration; i += Time.deltaTime)
        {
            _moneyText.text = Mathf.Ceil(Mathf.Lerp(_basePlayerMoney, _newPlayerMoney, i)).ToString();
            yield return null;
        }

    }

    public void SetupGame()
    {
        #region Value Settings
        _basePlayerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        _highscore = PlayerPrefs.GetInt("Highscore", 0);
        _totalHits = 0;
        #endregion

        #region Objects Activations
        _endGameUI.gameObject.SetActive(false);
        _newHighScoreText.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
        _scoreText.gameObject.SetActive(true);
        #endregion

        #region Texts Settings
        _moneyText.text = _basePlayerMoney.ToString();
        _scoreText.text = _totalHits.ToString() + "\nHits";
        _highscoreText.text = _highscore.ToString();
        #endregion

        _gameLose = false;
        _ball.ResetPosition();
        _playerRacket.ResetPosition();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
