using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.IO;

public class LogoVolume : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/settingsData.json"))
        {
            SettingsData loadData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(Application.persistentDataPath + "/settingsData.json"));

            mixer.SetFloat("MasterVolume", Mathf.Log10(loadData.masterValue) * 20);

            mixer.SetFloat("MusicVolume", Mathf.Log10(loadData.musicValue) * 20);

            mixer.SetFloat("SFXVolume", Mathf.Log10(loadData.sfxValue) * 20);

            Debug.Log("Volume Successfully Loaded");
        }
    }
}
