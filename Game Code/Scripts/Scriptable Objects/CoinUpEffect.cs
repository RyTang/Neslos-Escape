using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffect/CoinUpEffect")]
public class CoinUpEffect : PowerUpEffect
{
    public int amount;

    public override void Apply(GameObject target) {
        GameController.Controller.gameData.Coins += amount;
        AudioManager.instance.Play("Coin_PickUp");
    }
}
