using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public AudioSound[] sounds;
    public AudioSound[] themes;
    private AudioSound currentTheme;
    public static AudioManager instance;
    public float transitionDuration = 0.5f;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (AudioSound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            // TODO: Need to create a way to link to game settings
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (AudioSound t in themes) {
            t.source = gameObject.AddComponent<AudioSource>();
            t.source.clip = t.clip;

            // TODO: Need to create a way to link to game settings
            t.source.volume = t.volume;
            t.source.pitch = t.pitch;
            t.source.loop = t.loop;
        }
    }

    public void Play(string name) {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogError(String.Format("Audio Manager: Unable to find audio clip '{0}'", name));
            return;
        }
        s.source.Play();
    }

    /// <summary>
    /// Used to select a song from the selections with transition effect
    /// </summary>
    /// <param name="name">Name of the theme title</param>
    public void ChangeBackgroundThemeWithTransition(string name) {
        if (currentTheme != null && name == currentTheme.name) { // Ignore if current theme playing arldy
            return;
        }
        
        AudioSound theme = Array.Find(themes, theme => theme.name == name);
        // TODO: Might create enum to find the desired theme instead of using string -> Due to easy mistakes with typing strings
        if (theme == null) {
            Debug.LogError(String.Format("Audio Manager: Unable to find desired background Theme '{0}'", name));
            return;
        }

        StartCoroutine(FadeAudioInAndOut(theme));
    }

    /// <summary>
    /// Used to select a song from the selections
    /// </summary>
    /// <param name="name">Name of the theme title</param>
    public void ChangeBackgroundTheme(string name) {
        if (currentTheme != null && name == currentTheme.name) { // Ignore if current theme playing arldy
            return;
        }

        AudioSound theme = Array.Find(themes, theme => theme.name == name);
        // TODO: Might create enum to find the desired theme instead of using string -> Due to easy mistakes with typing strings
        if (theme == null) {
            Debug.LogError(String.Format("Audio Manager: Unable to find desired background Theme '{0}'", name));
            return;
        }

        currentTheme.source.Stop();
        currentTheme = theme;
        currentTheme.source.volume = 1;
        currentTheme.source.Play();
    }

    private IEnumerator FadeAudioInAndOut(AudioSound nextTheme) {
        float currentTime = 0;
        if (currentTheme != null && currentTheme.source.isPlaying) {  // Stop the theme if it's about to override something
            while (currentTime < transitionDuration/2) {
                currentTime += Time.deltaTime;
                currentTheme.source.volume = Mathf.Lerp(1, 0, currentTime / (transitionDuration / 2));
                yield return null;
            }
            currentTheme.source.Stop();
        }
        currentTheme = nextTheme;
        currentTheme.source.volume = 0;
        currentTheme.source.Play();
        currentTime = 0;
        while (currentTime < transitionDuration / 2) {
            currentTime += Time.deltaTime;
            currentTheme.source.volume = Mathf.Lerp(0, 1, currentTime / (transitionDuration / 2));
            yield return null;
        }
    }


    public string GetCurrentBackgroundTheme() {
        return currentTheme.name;
    }

    /// <summary>
    /// Used for when no curren background is wanted to be played
    /// </summary>
    public void ChangeBackgroundTheme() {
        if (currentTheme != null && currentTheme.source.isPlaying) {  // Stop the theme if there's any
            currentTheme.source.Stop();
        }
    }
}
