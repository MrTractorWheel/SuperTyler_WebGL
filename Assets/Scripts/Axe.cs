using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] float axeSpeed = 20f;
    Rigidbody2D rigbody;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * axeSpeed;
    }

    void Update()
    {
        rigbody.velocity = new Vector2(xSpeed,0f);
        transform.localScale = new Vector2(Mathf.Sign(rigbody.velocity.x), 1f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            Destroy(other.gameObject);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Coin")) return;
        gameObject.SetActive(false);
    }
}