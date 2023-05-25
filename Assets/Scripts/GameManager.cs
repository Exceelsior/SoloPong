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
    private Sprite _pauseGameSprite;
    [SerializeField]
    private Sprite _unpauseGameSprite;
    [SerializeField]
    private GameObject _pauseUI;
    [SerializeField]
    private TextMeshProUGUI _highscoreText;
    [SerializeField]
    private TextMeshProUGUI _newHighScoreText;
    [SerializeField]
    private TextMeshProUGUI _endGameScoreText;
    [SerializeField]
    private AudioManager _audioManager;


    [SerializeField]
    private float _moneyGainAnimationDuration = 1.0f;
    [SerializeField]
    private AnimationCurve _scoreGainScaleAnimationCurve;
    [SerializeField]
    private float _ballLoseHeight = -0.5f;
    [SerializeField]
    private CustomizationManager _customizationManager;
    [SerializeField]
    private AnalyticsManager _analyticsManager;
    
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
        
        #region Skin Initialization
        ShopItem racketItem = _customizationManager.GetCurrentRacketSkin();
        if (racketItem.itemPrefab != null)
        {
            Instantiate(racketItem.itemPrefab, _playerRacket.racketMeshParent);
        }

        ShopItem ballItem = _customizationManager.GetCurrentBallSkin();
        if (ballItem.itemPrefab != null)
        {
            Instantiate(ballItem.itemPrefab, _ball.transform);
        }

        ShopItem ballTrailItem = _customizationManager.GetCurrentTrailSkin();
        if (ballTrailItem.itemPrefab != null)
        {
            Instantiate(ballTrailItem.itemPrefab, _ball.transform);
        }
        #endregion

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
        _pauseButton.image.sprite = _gamePaused ? _unpauseGameSprite : _pauseGameSprite;
        _pauseUI.SetActive(_gamePaused);
    }
    private void GameLose()
    {
        AdsManager.instance.HideBannerAd();
        _gameLose = true;

        _analyticsManager?.ThrowGameEndEvent(_totalHits);

        _audioManager.PlayGameLoseSound();

        _newPlayerMoney = _basePlayerMoney + _totalHits;
        PlayerPrefs.SetInt("Money", _newPlayerMoney);
        if (_totalHits > _highscore) PlayerPrefs.SetInt("Highscore", _totalHits);
        _endGameScoreText.text = _totalHits.ToString() + "\nHITS!";

        _pauseButton.gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(false);
        _endGameUI.SetActive(true);
        _newHighScoreText.gameObject.SetActive(_totalHits > _highscore);

        _ball.ResetPosition();
        _playerRacket.ResetPosition();

        

        StartCoroutine(MoneyGainCoroutine());
    }

    private void IncrementScore()
    {
        if(_scoreGainCoroutine != null)
        {
            StopCoroutine(_scoreGainCoroutine);
        }

        _totalHits++;
        _scoreText.text = _totalHits.ToString() + "\nHITS!";

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
        AdsManager.instance.LoadBanner();

        if (AdsManager.instance.OnShowAdsComplete.GetInvocationList().Length > 0)
        {
            AdsManager.instance.OnShowAdsComplete -= () => SetupGame();
            AdsManager.instance.OnShowAdsComplete = null;
        }

        #region Value Settings
        _basePlayerMoney = PlayerPrefs.GetInt("Money", 0);
        _highscore = PlayerPrefs.GetInt("Highscore", 0);
        _totalHits = 0;
        _gameLose = false;
        _gamePaused = false;
        #endregion

        #region Objects Activations
        _endGameUI.gameObject.SetActive(false);
        _pauseUI.gameObject.SetActive(false);
        _newHighScoreText.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
        _scoreText.gameObject.SetActive(true);
        #endregion

        #region Texts Settings
        _moneyText.text = _basePlayerMoney.ToString();
        _scoreText.text = _totalHits.ToString() + "\nHITS!";
        _highscoreText.text = _highscore.ToString();
        #endregion
    }

    public void Retry()
    {
        AdsManager.instance.OnShowAdsComplete += () => SetupGame();
        AdsManager.instance.LoadAdInterstitial();
    }

    public void BackToMenu()
    {
        AdsManager.instance.OnShowAdsComplete += () => ChangeSceneToMenu();
        AdsManager.instance.LoadAdInterstitial();
    }

    private void ChangeSceneToMenu()
    {
        AdsManager.instance.OnShowAdsComplete -= () => ChangeSceneToMenu();
        AdsManager.instance.OnShowAdsComplete = null;
        SceneManager.LoadScene("MenuScene");
    }
}
