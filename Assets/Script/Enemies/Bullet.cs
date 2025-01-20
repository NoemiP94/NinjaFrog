using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    [SerializeField]
    float speed = 1; //velocità
    Vector3 direction;
    
    Rigidbody2D rb;
    bool isDestroyed = false;

    private void Awake()
    {
        instance = this;
        
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    public void Init(Vector3 dir, float speed=1)
    {
        direction = dir;
        this.speed = speed;
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDestroyed)
        {
            if (collision.tag == "Player")
            {
                //fa danno al giocatore
                Player.instance.health.TakeDamage();
                DestroyBullet();
            }
            if(collision.gameObject.layer == LayerMask.NameToLayer("DestroyPoint"))
            {
                DestroyBullet();
            }

        }
    }

    private void DestroyBullet()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }

}
