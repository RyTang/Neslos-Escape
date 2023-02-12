using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameLevels {
    START = 0,
    GAME = 1
}

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager Manager { get; private set; }

    public GameObject loadingScreen;
    public Animator animator;
    public GameData currentGameData;
    public float transitionTime = 1;

    private void Awake() {
        if (Manager != null) {
            Destroy(gameObject);
            return;
        }

        Manager = this;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;

    }

    public void LoadGameLevel(GameLevels level) {
        switch (level) {
            case GameLevels.START:
                AudioManager.instance.ChangeBackgroundThemeWithTransition("Starting_Theme");
                break;
            case GameLevels.GAME:
                AudioManager.instance.ChangeBackgroundThemeWithTransition("Annoying_Theme");
                ResetGameData();
                break;
        }

        StartCoroutine(LoadLevel(level));
    }

    public void ResetGameData() {
        PlayerDataLoader.UpdateScoreData(currentGameData);

        currentGameData.Score = 0;
        currentGameData.Coins = 0;
    }

    private IEnumerator LoadLevel(GameLevels level) {
        // TODO: Play Loading Screen Animation
        animator.SetTrigger("Loading");
        yield return new WaitForSecondsRealtime(transitionTime);
        Time.timeScale = 0;  // Stop any Game Logic

        yield return SceneManager.LoadSceneAsync((int)level);

        animator.SetTrigger("Finish");

        yield return new WaitForSecondsRealtime(transitionTime);
        Time.timeScale = 1; // Start Game logic when ready
    }

}
