using UnityEngine;

public class Machette : MonoBehaviour
{
    [SerializeField] float machetteSpeed = 20f;
    Rigidbody2D rigbody;
    PlayerMovement player;
    float xSpeed;
    public DeathEffect deathEffect;
    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        rigbody.velocity = new Vector2(xSpeed,0f);
        transform.localScale = new Vector2(Mathf.Sign(rigbody.velocity.x), 1f);
    }

    public void setXSpeed(float speed){
        xSpeed = speed * machetteSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            deathEffect.PlayDeathEffect(other.gameObject);
            FindObjectOfType<VolumeSettings>().playEnemyDeathSFX();
            Destroy(other.gameObject);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Coin")) return;
        gameObject.SetActive(false);
    }
}
