using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptables/Item")]
public class ShopItem : ScriptableObject
{
    public GameObject itemPrefab;
    public Sprite itemSprite;
    public int requiredMoneyToEquip;
}
