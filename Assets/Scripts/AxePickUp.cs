using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AxePickUp : MonoBehaviour
{
    [SerializeField] int numToPick = 5;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] float messageDuration = 0.5f;
    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !wasCollected){
            wasCollected = true;
            FindObjectOfType<GameSession>().TakeAxe(numToPick);
            GameObject messageInstance = Instantiate(messagePrefab, FindObjectOfType<Canvas>().transform);
            TextMeshProUGUI textMeshPro = messageInstance.GetComponent<TextMeshProUGUI>();
            AudioSource sfx = messageInstance.GetComponent<AudioSource>();
            sfx.Play();
            textMeshPro.text = "5 Axes Picked-Up";
            RectTransform rectTransform = messageInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
            Destroy(messageInstance, messageDuration);
            if(FindObjectOfType<GameSession>().machetteCount == 0)  
                other.GetComponent<PlayerMovement>().ChangeWeaponOnPickup(true);
            Destroy(gameObject);
        }
    }
}
