using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rigbody;

    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigbody.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Coin")) return;
        moveSpeed = -moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy(){
        transform.localScale = new Vector2(-Mathf.Sign(rigbody.velocity.x), 1f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            moveSpeed = 0;
            if(other.gameObject.transform.position.x < transform.position.x){
                FlipEnemy();
            }
        }
    }
}
