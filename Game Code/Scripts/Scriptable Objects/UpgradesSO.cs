using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum purchaseType{
    rollUpgrade,
    knockbackUpgrade,
    recoverUpgrade,
    laserPurchase,
    secondLifeUpgrade,
    pokemonRockPaperScissors,
}

[CreateAssetMenu(fileName = "Upgrades Data")]
public class UpgradesSO : ScriptableObject
{

    [Header("Permanent Upgrade")]
    [Range(0,5)]
    public int rollLevel;
    [Range(0,5)]
    public int knockbackLevel;
    [Range(0,5)]
    public int recoveryLevel;

    [Header("One Time Usage")]
    [Range(0,5)]
    public int laserCount;
    public bool secondLife;
    public bool pokemonRockPaperScissors;
}
