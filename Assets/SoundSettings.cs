using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/settingsData.json"))
        {
            LoadSettings();
        }
        else
        {
            SetMaster(0.5f);
            SetMusic(0.5f);
            SetSFX(0.5f);
        }
    }

    public void SetMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        SaveSettings();
    }

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        SaveSettings();
    }

    public void SetSFX(float sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        SaveSettings();
    }

    void SaveSettings()
    {
        SettingsData settingsData = new SettingsData();

        settingsData.masterValue = masterSlider.value;
        settingsData.musicValue = musicSlider.value;
        settingsData.sfxValue = SFXSlider.value;

        string dataString = JsonUtility.ToJson(settingsData);
        File.WriteAllText(Application.persistentDataPath + "/settingsData.json", dataString);

        Debug.Log("Settings Successfully Saved");
    }

    void LoadSettings()
    {
        SettingsData loadData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(Application.persistentDataPath + "/settingsData.json"));

        masterSlider.value = loadData.masterValue;
        mixer.SetFloat("MasterVolume", Mathf.Log10(loadData.masterValue) * 20);

        musicSlider.value = loadData.musicValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(loadData.musicValue) * 20);

        SFXSlider.value = loadData.sfxValue;
        mixer.SetFloat("SFXVolume", Mathf.Log10(loadData.sfxValue) * 20);

        Debug.Log("Settings Successfully Loaded");
    }
}

[System.Serializable]
public class SettingsData
{
    public float masterValue;
    public float musicValue;
    public float sfxValue;
}