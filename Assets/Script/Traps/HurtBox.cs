using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField]
    float force = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //se l'ggetto che entra in collisione è il Player
        if(collision.tag == "Player")
        {
            Health h = collision.GetComponent<Health>(); //cerchiamo lo script della salute
            if (h != null) //se la salute non è null
            { 
                h.TakeDamage();
                Player.instance.KnockBack(force, transform);
            }
        }
    }
}
