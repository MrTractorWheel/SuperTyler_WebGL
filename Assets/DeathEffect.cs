using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject deathEffectPrefab; 
    [SerializeField] AudioClip deathSFX;
    public void PlayDeathEffect()
    {
        Instantiate(deathEffectPrefab, FindObjectOfType<PlayerMovement>().gameObject.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        FindObjectOfType<ParticleSystem>().Play();
    }
}
