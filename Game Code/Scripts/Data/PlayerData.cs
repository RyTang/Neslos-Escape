using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to Store Player Data
/// </summary>
public class PlayerData
{
    public int totalCoins;
    public float highScore;

    public PlayerData(int coins, float highScore)
    {
        totalCoins = coins;
        this.highScore = highScore;
    }
}

/// <summary>
/// Used to load and save data to a persistent state
/// </summary>
public static class PlayerDataLoader
{
    public static void UpdateScoreData(GameData data)
    {
        float highScore = PlayerPrefs.HasKey("High_Score") && PlayerPrefs.GetFloat("High_Score") > data.Score ? PlayerPrefs.GetFloat("High_Score") : data.Score;
        int highCoins = PlayerPrefs.GetInt("Total_Coins") + data.Coins;
        PlayerPrefs.SetFloat("High_Score", highScore);
        PlayerPrefs.SetInt("Total_Coins", highCoins);
    }

    /// <summary>
    /// Updates all data except for high Score
    /// </summary>
    /// <param name="data"></param>
    public static void UpdateData(PlayerData data)
    {
        int TotalCoins = data.totalCoins;
        PlayerPrefs.SetInt("Total_Coins", TotalCoins);
    }

    public static PlayerData LoadPrefs()
    {
        float highScore = PlayerPrefs.GetFloat("High_Score");
        int highCoins = PlayerPrefs.GetInt("Total_Coins");
        return new PlayerData(highCoins, highScore);
    }

}
