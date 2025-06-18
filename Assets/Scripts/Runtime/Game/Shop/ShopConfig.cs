using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Config", menuName = "Config/Shop Config")]
public class ShopConfig : BaseSettings
{
    public List<ShopItemData> BackgroundItems = new List<ShopItemData>();
    public List<FieldItemData> FieldItems = new List<FieldItemData>();
    public Sprite SelectedItemSprite;
    public Sprite NotSelectedItemSprite;
}

[Serializable]
public class ShopItemData
{
    public Sprite Sprite;
    public int Price;
}

[Serializable]
public class FieldItemData : ShopItemData
{
    public Sprite HeaderSprite;
}