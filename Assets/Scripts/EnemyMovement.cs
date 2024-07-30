using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rigbody;
    
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isInScene()) rigbody.velocity = new Vector2(moveSpeed, 0);
        else rigbody.velocity = Vector2.zero;
    }

    bool isInScene(){
        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraHalfHeight = mainCamera.orthographicSize;
        Rect cameraRect = new Rect(
            cameraPos.x - cameraHalfWidth * 2,
            cameraPos.y - cameraHalfHeight * 2,
            cameraHalfWidth * 4,
            cameraHalfHeight * 4
        );
        if(cameraRect.Contains(transform.position)) {
            animator.SetBool("isAnimActive", true);
            return true;
        }
        else {
            animator.SetBool("isAnimActive", false);
            return false;
        }
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
