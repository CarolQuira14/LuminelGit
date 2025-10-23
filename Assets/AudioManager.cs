using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de audio")]
    public AudioSource musicaSource;
    public AudioSource efectosSource;

    [Header("Listas de música y efectos")]
    public List<AudioClip> pistasMusica;
    public List<AudioClip> efectosSonido;

    [Header("Configuración de volumen y fade")]
    [Range(0f, 1f)] public float volumenMusica = 1f;
    [Range(0f, 1f)] public float volumenEfectos = 1f;
    public float fadeDuration = 1f;

    private Dictionary<string, AudioClip> musicaDict;
    private Dictionary<string, AudioClip> efectosDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Crear diccionarios para buscar por nombre
        musicaDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in pistasMusica)
        {
            if (!musicaDict.ContainsKey(clip.name))
                musicaDict.Add(clip.name, clip);
        }

        efectosDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in efectosSonido)
        {
            if (!efectosDict.ContainsKey(clip.name))
                efectosDict.Add(clip.name, clip);
        }

    }

    //Reproducir música con fade
    public void PlayMusicaConFade(string nombreMusica)
    {
        if (musicaDict.TryGetValue(nombreMusica, out AudioClip nuevaMusica))
        {
            if (musicaSource.clip == nuevaMusica) return; // Evita cambiar si ya está sonando
            StopAllCoroutines();
            StartCoroutine(FadeMusica(nuevaMusica));
        }
        else
        {
            Debug.LogWarning("No se encontró la música: " + nombreMusica);
        }
    }

    private IEnumerator FadeMusica(AudioClip nuevaMusica)
    {
        // Fade Out
        float startVol = musicaSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicaSource.volume = Mathf.Lerp(startVol, 0, t / fadeDuration);
            yield return null;
        }
        musicaSource.volume = 0;

        // Cambiar música
        musicaSource.clip = nuevaMusica;
        musicaSource.Play();

        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicaSource.volume = Mathf.Lerp(0, volumenMusica, t / fadeDuration);
            yield return null;
        }
        musicaSource.volume = volumenMusica;
    }

    // 🔊 Reproducir efecto
    public void PlayEfecto(string nombreEfecto)
    {
        if (efectosDict.TryGetValue(nombreEfecto, out AudioClip clip))
        {
            efectosSource.PlayOneShot(clip, volumenEfectos);
        }
        else
        {
            Debug.LogWarning("No se encontró el efecto de sonido: " + nombreEfecto);
        }
    }
}
