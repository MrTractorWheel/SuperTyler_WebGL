using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject deathEffectPrefab; 
    [SerializeField] AudioClip deathSFX;
    public void PlayDeathEffect(GameObject gameObject)
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        Instantiate(deathEffectPrefab, gameObject.transform.position, Quaternion.identity);
        FindObjectOfType<ParticleSystem>().Play();
    }
}
