using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private void Update()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().volume = (float) PlayerPrefs.GetInt("SoundEffectVolume")/100;
        GameObject.Find("ThemeSong").GetComponent<AudioSource>().volume = (float) PlayerPrefs.GetInt("MusicVolume")/100;
    }
}
