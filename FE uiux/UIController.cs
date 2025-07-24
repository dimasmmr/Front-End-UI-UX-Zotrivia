using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public TextMeshProUGUI qualityLabel;

    void Start()
    {
        // Load saved values or set default if no value exists
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f); // Default volume = 1 (maksimum)
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Apply saved values to the AudioManager
        AudioManager.instance.MusicVolume(_musicSlider.value);
        AudioManager.instance.SFXVolume(_sfxSlider.value);

        // Load and apply saved quality level
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", 2); // Default quality = medium (level 2)
        QualitySettings.SetQualityLevel(savedQualityLevel);
        UpdateQualityLabel();

        // Add listeners to sliders
        _musicSlider.onValueChanged.AddListener(delegate { SaveMusicVolume(); });
        _sfxSlider.onValueChanged.AddListener(delegate { SaveSFXVolume(); });
    }

    void UpdateQualityLabel()
    {
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", 2); // Default quality = medium (level 2)
        switch (savedQualityLevel)
        {
            case 0:
                qualityLabel.text = "Quality: Low";
                break;
            case 2:
                qualityLabel.text = "Quality: Medium";
                break;
            case 3:
                qualityLabel.text = "Quality: High";
                break;
            default:
                qualityLabel.text = "Quality: Medium";
                break;
        }
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }

    void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        PlayerPrefs.Save(); // Make sure to save changes
    }

    void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
        PlayerPrefs.Save(); // Make sure to save changes
    }

    // Quality settings methods
    public void low()
    {
        QualitySettings.SetQualityLevel(0);
        PlayerPrefs.SetInt("QualityLevel", 0); // Save quality level
        PlayerPrefs.Save();
        UpdateQualityLabel();
    }

    public void med()
    {
        QualitySettings.SetQualityLevel(2);
        PlayerPrefs.SetInt("QualityLevel", 2); // Save quality level
        PlayerPrefs.Save();
        UpdateQualityLabel();
    }

    public void high()
    {
        QualitySettings.SetQualityLevel(3);
        PlayerPrefs.SetInt("QualityLevel", 3); // Save quality level
        PlayerPrefs.Save();
        UpdateQualityLabel();
    }
}
