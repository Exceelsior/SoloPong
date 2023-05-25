using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public ShopItem shopItem { get; private set; }
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemPriceText;
    [SerializeField]
    private Image _itemLockedOrEquippedImage;
    [SerializeField]
    private Sprite _itemLockedSprite;
    [SerializeField]
    private Sprite _itemEquippedSprite;

    private ShopManager _shopManager;
    

    public void InitButton(ShopItem item, ShopManager shopManager)
    {
        shopItem = item;
        _shopManager = shopManager;
        _itemImage.sprite = shopItem.itemSprite;
        _itemPriceText.text = shopItem.requiredMoneyToEquip.ToString();

        if(shopItem.requiredMoneyToEquip > shopManager.Money)
        {
            _itemLockedOrEquippedImage.enabled = true;
            _itemLockedOrEquippedImage.sprite = _itemLockedSprite;
            _itemLockedOrEquippedImage.preserveAspect = true;
            _itemLockedOrEquippedImage.SetNativeSize();
        }
        else if(_shopManager.CheckCurrentEquippedItem(item))
        {
            _itemLockedOrEquippedImage.enabled = true;
            _itemLockedOrEquippedImage.sprite = _itemEquippedSprite;
            _itemLockedOrEquippedImage.preserveAspect = true;
            _itemLockedOrEquippedImage.SetNativeSize();
        }
        else
        {
            _itemLockedOrEquippedImage.enabled = false;
        }
        

    }

    public void SelectItem()
    {
        if(_shopManager.Money >= shopItem.requiredMoneyToEquip)
        {
            _shopManager.EquipItem(shopItem);
            _itemLockedOrEquippedImage.sprite = _itemEquippedSprite;
            _itemLockedOrEquippedImage.enabled = true;
        }
        
    }

    public void DeselectItem()
    {
        _itemLockedOrEquippedImage.enabled = false;
    }

}
