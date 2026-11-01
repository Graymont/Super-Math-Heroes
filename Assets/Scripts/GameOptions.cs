using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public int musicVolume = 100;
    public int soundEffectVolume = 100;

    public int maxVolume = 100;
    public int minVolume = 0;

    public Slider musicVolumeSlider;
    public Slider soundEffectVolumeSlider;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI soundEffectText;

    private void Start()
    {
        musicVolume = PlayerPrefs.GetInt("MusicVolume", 100);
        soundEffectVolume = PlayerPrefs.GetInt("SoundEffectVolume", 100);
    }
    private void Update()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.maxValue = maxVolume;
            musicVolumeSlider.value = musicVolume;
        }
        if (soundEffectVolumeSlider != null)
        {
            soundEffectVolumeSlider.maxValue = maxVolume;
            soundEffectVolumeSlider.value = soundEffectVolume;
        }

        if (musicText != null && soundEffectText != null)
        {
            musicText.text = musicVolume + "";
            soundEffectText.text = soundEffectVolume + "";
        }
    }

    public void SetMusicVolume(int val)
    {
        musicVolume = Mathf.Clamp(val, minVolume, maxVolume);

        SaveVolumes();
    }

    public void SetEffectVolume(int val)
    {
        soundEffectVolume = Mathf.Clamp(val, minVolume, maxVolume);
        SaveVolumes();
    }

    public void IncreaseMusicVolume(int amount = 10)
    {
        musicVolume = Mathf.Clamp(musicVolume + amount, minVolume, maxVolume);
        SaveVolumes();
    }

    public void DecreaseMusicVolume(int amount = 10)
    {
        musicVolume = Mathf.Clamp(musicVolume - amount, minVolume, maxVolume);
        SaveVolumes();
    }

    public void IncreaseEffectVolume(int amount = 10)
    {
        soundEffectVolume = Mathf.Clamp(soundEffectVolume + amount, minVolume, maxVolume);
        SaveVolumes();
    }

    public void DecreaseEffectVolume(int amount = 10)
    {
        soundEffectVolume = Mathf.Clamp(soundEffectVolume - amount, minVolume, maxVolume);
        SaveVolumes();
    }

    public void SaveVolumes()
    {
        PlayerPrefs.SetInt("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("SoundEffectVolume", soundEffectVolume);
    }
}