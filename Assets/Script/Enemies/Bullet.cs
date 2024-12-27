using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed = 1; //velocità
    Vector3 direction;


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
        if(collision.tag == "Player")
        {
            //fa danno al giocatore
            Player.instance.health.TakeDamage();
        }  
    }

}
