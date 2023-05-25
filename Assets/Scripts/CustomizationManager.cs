using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationManager : MonoBehaviour
{
    [SerializeField]
    private ShopItem[] _racketSkins;
    [SerializeField]
    private ShopItem[] _ballSkins;
    [SerializeField]
    private ShopItem[] _ballTrailSkins;

    public ShopItem GetCurrentRacketSkin()
    {
        ShopItem result = new ShopItem();
        int skinIndex = PlayerPrefs.GetInt("RacketSkin", 0);
        if (skinIndex >= 0 && skinIndex < _racketSkins.Length)
        {
            result = _racketSkins[skinIndex];
        }
        return result;
    }

    public ShopItem GetCurrentBallSkin()
    {
        ShopItem result = new ShopItem();
        int skinIndex = PlayerPrefs.GetInt("BallSkin", 0);
        if (skinIndex >= 0 && skinIndex < _ballSkins.Length)
        {
            result = _ballSkins[skinIndex];
        }
        return result;
    }

    public ShopItem GetCurrentBallTrailSkin()
    {
        ShopItem result = new ShopItem();
        int skinIndex = PlayerPrefs.GetInt("BallTrailSkin", 0);
        if (skinIndex >= 0 && skinIndex < _ballTrailSkins.Length)
        {
            result = _ballTrailSkins[skinIndex];
        }
        return result;
    }

    public void SetRacketSkin(int newSkinIndex)
    {
        PlayerPrefs.SetInt("RacketSkin", newSkinIndex);
    }

    public void SetBallSkin(int newSkinIndex)
    {
        PlayerPrefs.SetInt("BallSkin", newSkinIndex);
    }

    public void SetBallTrailSkin(int newSkinIndex)
    {
        PlayerPrefs.SetInt("BallTrailSkin", newSkinIndex);
    }
}