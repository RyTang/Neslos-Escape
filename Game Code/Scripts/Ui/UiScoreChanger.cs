using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiScoreChanger : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public string prefix;
    public GameData gameData;

    public void FixedUpdate() {
        textUI.SetText(prefix + " " + (int) gameData.Score);
    }
}
