using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    PickUpType type = PickUpType.hp;
    [SerializeField]
    GameObject Effect = null; //generiamo l'effetto
    [SerializeField]
    Item itemToGive = null;

    private void Start()
    {
        if (itemToGive != null) 
        {
            //cerchiamo l'immagine e gli diamo quella dell'item 
            GetComponentInChildren<SpriteRenderer>().sprite = itemToGive.image;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Player.instance != null && Player.instance.health != null) 
            {
                switch (type)
                {
                    case PickUpType.hp:
                        Player.instance.health.Heal(); //cura il player
                        if(UIManager.instance != null)
                        {
                            UIManager.instance.ShowHp(); //mostra gli hp
                        }
                        break;
                    case PickUpType.hpmax:
                        Player.instance.health.AddNewLife(); //aggiungiamo salute
                        if(UIManager.instance != null)
                        {
                            UIManager.instance.AddNewLife(); //aggiungi una nuova vita
                        }
                        break;
                    case PickUpType.coin:
                        if(LevelManager.instance != null)
                        {
                            LevelManager.instance.AddCoin(); //aggiungiamo una moneta
                        }
                        break;
                    case PickUpType.item:
                        if(itemToGive != null)
                        {
                            LevelManager.instance.AddItem(itemToGive);
                        }
                        break;
                }
                //se l'effetto non è null, prima di distruggere l'oggetto generiamo l'effetto
                if (Effect != null) 
                    Instantiate(Effect, transform.position, Quaternion.identity); //passiamo la posizione e la rotazione teniamo quella dell'oggetto(Quaternion.identity)
            }
            Destroy(gameObject);
        }
    }

    enum PickUpType
    {
        hp,
        hpmax, 
        coin,
        item
    }
}
