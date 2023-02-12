using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Controller { get; private set; }
    [SerializeField] private float scrollSpeed = 0.75f;
    [SerializeField] private float scrollSpeedUp = 1;
    [SerializeField] private float maxScrollSpeed = 2;
    public GameData gameData;
   

    public float ScrollSpeed {
        get { return scrollSpeed; }
        set { scrollSpeed = value; }
    }

    private void Start() {
        if (Controller == null) {
            Controller = this;
        }
    }

    private void FixedUpdate() {
        if (scrollSpeed <= maxScrollSpeed) {
            scrollSpeed += scrollSpeedUp * 0.00001f;
        }
        gameData.Score += ScrollSpeed;
    }

    /// <summary>
    /// Game Over Scenario
    /// Stops the game, open the Game Over Menu
    /// </summary>
    public void GameOver() {
        Time.timeScale = 0;
        UiScript.UI.OpenGameOverMenu();
        // TODO: Resets score afterwards and saves highscores
    }
}
