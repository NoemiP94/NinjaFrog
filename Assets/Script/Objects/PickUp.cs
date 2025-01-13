using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    PickUpType type = PickUpType.hp;
    [SerializeField]
    GameObject Effect = null; //generiamo l'effetto

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
                case PickUpType.coin:
                    LevelManager.instance.AddCoin(); //aggiungiamo una moneta
                    break;
            }
            //se l'effetto non è null, prima di distruggere l'oggetto generiamo l'effetto
            if (Effect != null) 
                Instantiate(Effect, transform.position, Quaternion.identity); //passiamo la posizione e la rotazione teniamo quella dell'oggetto(Quaternion.identity)

            Destroy(gameObject);
        }
    }

    enum PickUpType
    {
        hp, hpmax, coin
    }
}
