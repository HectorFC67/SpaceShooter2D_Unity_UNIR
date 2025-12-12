using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip gameOverMusic;

    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }
    }

    void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || audioSource == null) return;

        if (audioSource.isPlaying && audioSource.clip == clip) return;

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic, true);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic, true);
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic, false); 
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
