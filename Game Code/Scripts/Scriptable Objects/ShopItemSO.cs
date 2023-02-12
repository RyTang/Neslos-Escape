using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "New Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public Image image;
    public string description;
    public int cost;
    public purchaseType upgradeType;
}
