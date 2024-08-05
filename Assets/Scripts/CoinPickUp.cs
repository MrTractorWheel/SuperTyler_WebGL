using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] int pointsForPickup = 100;
    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !wasCollected){
            FindObjectOfType<VolumeSettings>().playCoinPickUpSFX();
            wasCollected = true;
            FindObjectOfType<GameSession>().ToScore(pointsForPickup);
            DeactivateCoinAsync().Forget();
        }
    }

    private async UniTaskVoid DeactivateCoinAsync()
    {
        await UniTask.Delay(100); 
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
