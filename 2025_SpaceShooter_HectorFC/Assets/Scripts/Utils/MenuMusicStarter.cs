using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance?.PlayMenuMusic();
    }
}
