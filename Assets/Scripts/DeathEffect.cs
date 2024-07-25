using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject deathEffectPrefab; 

    public void PlayDeathEffect(GameObject gameObject)
    {
        Instantiate(deathEffectPrefab, gameObject.transform.position, Quaternion.identity);
        FindObjectOfType<ParticleSystem>().Play();
    }
}
