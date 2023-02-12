using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData")]
public class GameData : ScriptableObject {
    public float Score;
    public int Coins;
    public UpgradesSO upgradeData;
}