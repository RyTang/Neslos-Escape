using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string themeToPlay;
    public void Start() {
        StartCoroutine(playMusic());
    }

    IEnumerator playMusic() {
        while (AudioManager.instance == null) {
            yield return null;
        }
        AudioManager.instance.ChangeBackgroundThemeWithTransition(themeToPlay);
    }
}
