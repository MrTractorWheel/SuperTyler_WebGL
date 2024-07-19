using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] GameObject axe;
    [SerializeField] Transform weapon;

    ParticleSystem blood;
    
    Vector2 moveInput;
    Rigidbody2D rigbody;
    Animator animator;
    BoxCollider2D playerFeetBox;
    float gravityScaleAtStart;
    bool isAlive = true;
    private PauseFSM pauseFSM;
    private bool isControlsActive = true;
    public DeathEffect deathEffect;

    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerFeetBox = GetComponent<BoxCollider2D>();
        blood = GetComponent<ParticleSystem>();
        gravityScaleAtStart = rigbody.gravityScale;
        pauseFSM = FindObjectOfType<PauseFSM>(); 
    }

    void Update()
    {
        if(!isAlive) return;
        if(pauseFSM != null && pauseFSM.CurrentState == PauseFSM.GameState.Play){
            Run();
            FlipSprite();
            ClimbLeader();
            Die();
        }
    }

    void OnMove(InputValue value){
        if(!isAlive || !isControlsActive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(!isAlive || !isControlsActive) return;
        if(!playerFeetBox.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if(value.isPressed){
            rigbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value){
        if(!isAlive || !isControlsActive) return;
        if(value.isPressed){
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if(bullet != null){
                bullet.transform.position = weapon.transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Axe>().setXSpeed(transform.localScale.x);
                bullet.SetActive(true);
            }
        }
    }

    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rigbody.velocity.y);
        rigbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(rigbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
            transform.localScale = new Vector2(Mathf.Sign(rigbody.velocity.x), 1f);
        }
    }

    void ClimbLeader(){
        if(!playerFeetBox.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            rigbody.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        rigbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(rigbody.velocity.x, moveInput.y * climbSpeed);
        rigbody.velocity = climbVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(rigbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die(){
        if(rigbody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))){
            isAlive = false;
            animator.SetTrigger("DieMF");
            deathEffect.PlayDeathEffect(gameObject);
            rigbody.velocity = Vector2.zero;
            rigbody.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill(){
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    public void ToggleControls(bool isActive){
        isControlsActive = isActive;
    }
}
