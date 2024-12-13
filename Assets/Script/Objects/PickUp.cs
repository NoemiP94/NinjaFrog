using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    PickUpType type = PickUpType.hp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (type)
            {
                case PickUpType.hp:
                    Player.instance.health.Heal(); //cura il player
                    UIManager.instance.ShowHp(); //mostra gli hp
                    break;
                case PickUpType.hpmax:
                    Player.instance.health.AddNewLife(); //aggiungiamo salute   
                    UIManager.instance.AddNewLife(); //aggiungi una nuova vita
                    break;
            }
            Destroy(gameObject);
        }
    }

    enum PickUpType
    {
        hp, hpmax
    }
}
