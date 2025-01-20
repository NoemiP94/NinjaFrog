using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField]
    float force = 1;
    public System.Action<Collision2D> OnCollisionEvent; //evento

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //se l'oggetto che entra in collisione è il Player
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnCollisionEvent != null)
        {
            OnCollisionEvent.Invoke(collision);
        }
    }
}
