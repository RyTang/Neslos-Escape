using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class AudioSound
{
    public string name;

    public AudioClip clip;

    [Range(0, 1)] public float volume;

    [Range(0.1f, 3)] public float pitch;

    [HideInInspector] public AudioSource source;

    public bool loop;
}
