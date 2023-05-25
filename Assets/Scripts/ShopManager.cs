using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int Money { get; private set; }

    [SerializeField]
    private CustomizationManager _customizationManager;
    [SerializeField]
    private TextMeshProUGUI _moneyText;
    [SerializeField]
    private RectTransform _racketItemButtonsParent;
    [SerializeField]
    private RectTransform _ballItemButtonsParent;
    [SerializeField]
    private RectTransform _trailItemButtonsParent;
    [SerializeField]
    private ItemButton _itemButtonPrefab;
    [SerializeField]
    private AudioManager _audioManager;

    private List<ItemButton> _ballSkinsButtons;
    private List<ItemButton> _racketSkinsButtons;
    private List<ItemButton> _trailSkinsButtons;

    void Awake()
    {
        Money = PlayerPrefs.GetInt("Money", 0);
        _moneyText.text = Money.ToString();

        _ballSkinsButtons = new List<ItemButton>();
        _racketSkinsButtons = new List<ItemButton>();
        _trailSkinsButtons = new List<ItemButton>();

        List<ShopItem> racketItems = _customizationManager.racketSkins;

        foreach(ShopItem item in racketItems)
        {
            ItemButton newRacketButton = Instantiate(_itemButtonPrefab, _racketItemButtonsParent);
            newRacketButton.InitButton(item, this);
            _racketSkinsButtons.Add(newRacketButton);
            
        }

        List<ShopItem> ballItems = _customizationManager.ballSkins;

        foreach (ShopItem item in ballItems)
        {
            ItemButton newBallButton = Instantiate(_itemButtonPrefab, _ballItemButtonsParent);
            newBallButton.InitButton(item, this);
            _ballSkinsButtons.Add(newBallButton);
        }

        List<ShopItem> trailItems = _customizationManager.trailSkins;

        foreach (ShopItem item in trailItems)
        {
            ItemButton newTrailButton = Instantiate(_itemButtonPrefab, _trailItemButtonsParent);
            newTrailButton.InitButton(item, this);
            _racketSkinsButtons.Add(newTrailButton);
        }

    }

    public void EquipItem(ShopItem newItem)
    {
        _audioManager.PlayUISound();

        ItemButton currentEquippedItemButton;
        ShopItem currentItem;

        switch (newItem.itemType)
        {
            case (ItemType.Ball):

                currentItem = _customizationManager.GetCurrentBallSkin();
                if (newItem == currentItem) return;

                currentEquippedItemButton = GetButtonMatchingWithItem(_ballSkinsButtons, currentItem);
                if (currentEquippedItemButton != null) currentEquippedItemButton.DeselectItem();
                _customizationManager.SetBallSkin(newItem);
                break;
            case (ItemType.Racket):

                currentItem = _customizationManager.GetCurrentRacketSkin();
                if (newItem == currentItem) return;

                currentEquippedItemButton = GetButtonMatchingWithItem(_racketSkinsButtons, currentItem);
                if (currentEquippedItemButton != null) currentEquippedItemButton.DeselectItem();
                _customizationManager.SetRacketSkin(newItem);
                break;
            case (ItemType.Trail):

                currentItem = _customizationManager.GetCurrentTrailSkin();
                if (newItem == currentItem) return;

                currentEquippedItemButton = GetButtonMatchingWithItem(_trailSkinsButtons, currentItem);
                if (currentEquippedItemButton != null) currentEquippedItemButton.DeselectItem();
                _customizationManager.SetBallTrailSkin(newItem);
                break;
        }
    }

    public bool CheckCurrentEquippedItem(ShopItem itemToCheck)
    {
        bool result = false;
        


        switch(itemToCheck.itemType)
        {
            case (ItemType.Ball):
                result = itemToCheck == _customizationManager.GetCurrentBallSkin();
                break;
            case (ItemType.Racket):
                result = itemToCheck == _customizationManager.GetCurrentRacketSkin();
                break;
            case (ItemType.Trail):
                result = itemToCheck == _customizationManager.GetCurrentTrailSkin();
                break;
        }
        return result;
    }

    ItemButton GetButtonMatchingWithItem(List<ItemButton> itemButtons, ShopItem item)
    {
        foreach(ItemButton button in itemButtons)
        {
            if (button.shopItem == item) return button;
        }
        return null;
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
