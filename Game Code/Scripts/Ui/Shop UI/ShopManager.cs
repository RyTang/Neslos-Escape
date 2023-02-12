using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TMP_Text coinText;
    public ShopItemSO[] shopItems;
    public ItemTemplate[] shopPanels;
    private PlayerData playerData;

    private void Start()
    {
        playerData = PlayerDataLoader.LoadPrefs();
        coinText.text = playerData.totalCoins.ToString();


        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].gameObject.SetActive(true);
        }



        CheckPurchaseable();
        LoadPanels();
    }

    public void MakePurchase(int coins)
    {
        if (playerData.totalCoins >= coins)
        {
            playerData.totalCoins -= coins;
            PlayerDataLoader.UpdateData(playerData);
        }
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (playerData.totalCoins >= shopItems[i].cost)
            {
                shopPanels[i].purchaseButton.interactable = true;
            }
            else
            {
                shopPanels[i].purchaseButton.interactable = false;
            }
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].titleText.text = shopItems[i].title;
            if (shopItems[i].image != null){
                shopPanels[i].itemImage.sprite = shopItems[i].image.sprite;

            }
            shopPanels[i].descriptionText.text = shopItems[i].description;
            shopPanels[i].coins.text = shopItems[i].cost.ToString() + " coins";
        }
    }
}
