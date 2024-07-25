using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start(){
        if(PlayerPrefs.HasKey("MusicVolume")){
            LoadMusicVolume();
        }
        else{
            SetMusicVolume();
        }
        if(PlayerPrefs.HasKey("SFXVolume")){
            LoadSFXVolume();
        }
        else{
            SetSFXVolume();
        }
    }

    public void SetMusicVolume(){
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadMusicVolume(){
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }

    public void SetSFXVolume(){
        float volume = musicSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadSFXVolume(){
        musicSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
    }
}
