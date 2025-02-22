using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{ 
    Player player;
    [SerializeField]
    float force = 3;
    private void Start()
        {
            player = Player.instance;
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy") //se la collisione � con un Enemy
        {
            player.Jump(force);
            Jumpable jump = collision.GetComponent<Jumpable>();
            if (jump != null)
            {
                jump.onJumpOn();
            }
            return;
        }
        else if(collision.tag == "Jumpable")
        {
            Jumpable jump = collision.GetComponent<Jumpable>();
            if (jump != null)
            {
                jump.onJumpOn();
            }
        }
    }
}
