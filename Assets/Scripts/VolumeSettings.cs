using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;
    
    [Header("---------- Audio Clip ----------")]
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] AudioClip weaponPickUpSFX;

    private void Start(){
        musicSource.clip = levelMusic;
        musicSource.Play();
    }
    
    public void playDeathSFX(){
        SFXsource.PlayOneShot(deathSFX);
    }

    public void playEnemyDeathSFX(){
        SFXsource.PlayOneShot(enemyDeathSFX);
    }

    public void playCoinPickUpSFX(){
        SFXsource.PlayOneShot(coinPickUpSFX);
    }

    public void playWeaponPickUpSFX(){
        SFXsource.PlayOneShot(weaponPickUpSFX);
    }
}
