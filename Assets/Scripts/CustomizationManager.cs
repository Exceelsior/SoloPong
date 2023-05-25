using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationManager : MonoBehaviour
{
    public List<ShopItem> racketSkins { get => _racketSkins;}
    public List<ShopItem> ballSkins { get => _ballSkins;}
    public List<ShopItem> trailSkins { get => _ballTrailSkins;}

    [SerializeField]
    private List<ShopItem> _racketSkins;
    [SerializeField]
    private List<ShopItem> _ballSkins;
    [SerializeField]
    private List<ShopItem> _ballTrailSkins;

    public ShopItem GetCurrentRacketSkin()
    {
        ShopItem result = ScriptableObject.CreateInstance<ShopItem>();
        int skinIndex = PlayerPrefs.GetInt("RacketSkin", 0);
        if (skinIndex >= 0 && skinIndex < _racketSkins.Count)
        {
            result = _racketSkins[skinIndex];
        }
        return result;
    }

    public ShopItem GetCurrentBallSkin()
    {
        ShopItem result = ScriptableObject.CreateInstance<ShopItem>();
        int skinIndex = PlayerPrefs.GetInt("BallSkin", 0);
        if (skinIndex >= 0 && skinIndex < _ballSkins.Count)
        {
            result = _ballSkins[skinIndex];
        }
        return result;
    }

    public ShopItem GetCurrentTrailSkin()
    {
        ShopItem result = ScriptableObject.CreateInstance<ShopItem>();
        int skinIndex = PlayerPrefs.GetInt("BallTrailSkin", 0);
        if (skinIndex >= 0 && skinIndex < _ballTrailSkins.Count)
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

    public void SetRacketSkin(ShopItem newSkin)
    {
        int newSkinIndex = _racketSkins.IndexOf(newSkin);
        PlayerPrefs.SetInt("RacketSkin", newSkinIndex);
    }

    public void SetBallSkin(ShopItem newSkin)
    {
        int newSkinIndex = _ballSkins.IndexOf(newSkin);
        PlayerPrefs.SetInt("BallSkin", newSkinIndex);
    }

    public void SetBallTrailSkin(ShopItem newSkin)
    {
        int newSkinIndex = _ballTrailSkins.IndexOf(newSkin);
        PlayerPrefs.SetInt("BallTrailSkin", newSkinIndex);
    }
}