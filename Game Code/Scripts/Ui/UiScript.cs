using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuStates {
    START,
    PAUSE,
    OPTIONS,
    GAME_OVER,
    PLAYING
}

public class UiScript : MonoBehaviour
{
    public static UiScript UI { get; private set; }
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject gameOverMenu;

    public MenuStates currentState;

    private string themeBeforePause;

    private void Awake() {
        UI = this;
    }

    private void Update() {
        switch (currentState) { // Might not be the best way to do this
            case MenuStates.START:
                /*AudioManager.instance.ChangeBackgroundTheme("Starting Theme");*/
                blocker.SetActive(false);
                startMenu.SetActive(true);
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                break;
            case MenuStates.PAUSE:
                blocker.SetActive(true);
                startMenu.SetActive(false);
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                break;
            case MenuStates.OPTIONS:
                blocker.SetActive(false);
                startMenu.SetActive(false);
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(true);
                gameOverMenu.SetActive(false);
                break;
            case MenuStates.GAME_OVER:
                blocker.SetActive(true);
                startMenu.SetActive(false);
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                gameOverMenu.SetActive(true);
                break;
            case MenuStates.PLAYING:
                blocker.SetActive(false);
                startMenu.SetActive(false);
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                break;
        }
    }

    public void OpenGameOverMenu() {
        currentState = MenuStates.GAME_OVER;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Starts the game initially
    /// </summary>
    public void StartButton() {
        // TODO: Starts Game Logic
        AudioManager.instance.Play("Button_Click");
        currentState = MenuStates.PLAYING;
        Time.timeScale = 1;
        GameLevelManager.Manager.LoadGameLevel(GameLevels.GAME);
    }

    /// <summary>
    /// Used to Unpause the Game
    /// </summary>
    public void ContinueButton() {
        AudioManager.instance.Play("Button_Click");
        if (themeBeforePause != "" || themeBeforePause != null) {
            AudioManager.instance.ChangeBackgroundThemeWithTransition(themeBeforePause);
        }
        currentState = MenuStates.PLAYING;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Resets Game Logic and Restarts the Level
    /// </summary>
    public void RestartButton() {
        // Possibly Store game data here
        AudioManager.instance.Play("Button_Click");
        GameLevelManager.Manager.LoadGameLevel(GameLevels.GAME);
    }

    /// <summary>
    /// Pauses the Game
    /// </summary>
    public void PauseButton() {
        AudioManager.instance.Play("Button_Click");
        if (AudioManager.instance.GetCurrentBackgroundTheme() != null) {
            themeBeforePause = AudioManager.instance.GetCurrentBackgroundTheme();
        }
        AudioManager.instance.ChangeBackgroundTheme("Pause_Music");
        currentState = MenuStates.PAUSE;
        Time.timeScale = 0;
    }


    /// <summary>
    /// Opens the Options Menu
    /// </summary>
    public void OptionsButton() {
        AudioManager.instance.Play("Button_Click");
        currentState = MenuStates.OPTIONS;
    }

    /// <summary>
    /// Returns to Pause Menu from Options
    /// </summary>
    public void OptionsBackButton() {
        AudioManager.instance.Play("Button_Click");
        currentState = MenuStates.START;
    }

    /// <summary>
    /// Returns user to Main Menu
    /// </summary>
    public void MainMenuButton() {
        AudioManager.instance.Play("Button_Click");
        GameLevelManager.Manager.LoadGameLevel(GameLevels.START);
    }

    /// <summary>
    /// Quits the Game
    /// </summary>
    public void QuitButton() {
        AudioManager.instance.Play("Button_Click");
        Application.Quit();
    }
}
