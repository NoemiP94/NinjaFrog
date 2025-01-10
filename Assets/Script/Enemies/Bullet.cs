using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    [SerializeField]
    float speed = 1; //velocit�
    Vector3 direction;
    
    Rigidbody2D rb;
    bool isDestroyed = false;

    private void Awake()
    {
        instance = this;
        
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector3 dir)
    {
        direction = dir;
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
                DestroyBullet();
                //fa danno al giocatore
                Player.instance.health.TakeDamage();
                
            }
        }
    }

    private void DestroyBullet()
    {
        isDestroyed = true;

        

        Destroy(gameObject);
    }

}
