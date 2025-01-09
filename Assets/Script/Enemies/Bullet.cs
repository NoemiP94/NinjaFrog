using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    [SerializeField]
    float speed = 1; //velocità
    Vector3 direction;
    protected Animator anim;
    Rigidbody2D rb;
    bool isDestroyed = false;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
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
                //fa danno al giocatore
                Player.instance.health.TakeDamage();
                DestroyBullet();
            }
        }
    }

    private void DestroyBullet()
    {
        isDestroyed = true;

        //anim.Play("destroy"); //NON PARTE L'ANIMAZIONE

        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }

}
