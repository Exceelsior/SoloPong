using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private CustomizationManager _customizationManager;
    [SerializeField]
    private Transform _racketParent;
    [SerializeField]
    private Transform _ballParent;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform _cameraTargetPoint;
    [SerializeField]
    private float _cameraRotationSpeed;
    [SerializeField]
    private TextMeshProUGUI _moneyText;
    [SerializeField]
    private TextMeshProUGUI _highscoreText;

    private void Start()
    {
        #region Skin Initialization
        ShopItem racketItem = _customizationManager.GetCurrentRacketSkin();
        if(racketItem.itemPrefab != null)
        {
            Instantiate(racketItem.itemPrefab, _racketParent);
        }

        ShopItem ballItem = _customizationManager.GetCurrentBallSkin();
        if (ballItem.itemPrefab != null)
        {
            Instantiate(ballItem.itemPrefab, _ballParent);
        }
        #endregion

        _moneyText.text = PlayerPrefs.GetInt("Money", 0).ToString();
        _highscoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    void Update()
    {
        _camera.transform.RotateAround(_cameraTargetPoint.position, Vector3.up, _cameraRotationSpeed * Time.deltaTime);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

}
