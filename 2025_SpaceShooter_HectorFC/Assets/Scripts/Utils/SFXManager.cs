using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] AudioClip allyShootClip;
    [SerializeField] AudioClip enemyShootClip;
    [SerializeField] AudioClip enemyExplodeClip;
    [SerializeField] AudioClip powerUpPickupClip;
    [SerializeField] AudioClip shieldHitClip;
    [SerializeField] AudioClip hitClip;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    public void PlayAllyShoot() => Play(allyShootClip);
    public void PlayEnemyShoot() => Play(enemyShootClip);
    public void PlayEnemyExplode() => Play(enemyExplodeClip);

    public void PlayPowerUpPickup() => Play(powerUpPickupClip);
    public void PlayShieldHit() => Play(shieldHitClip);
    public void PlayHit() => Play(hitClip);

    void Play(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
