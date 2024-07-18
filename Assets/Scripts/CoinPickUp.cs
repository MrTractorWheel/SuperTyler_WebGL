using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForPickup = 100;
    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !wasCollected){
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            wasCollected = true;
            FindObjectOfType<GameSession>().ToScore(pointsForPickup);
            Destroy(gameObject);
        }
    }
}
