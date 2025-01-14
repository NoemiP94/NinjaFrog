using UnityEngine;

public class Tax : MonoBehaviour,Interactable
{
    GameObject canvas;
    [SerializeField]
    RequestType type = RequestType.coin;
    [SerializeField]
    Item requestedItem = null;
    [SerializeField]
    int Import = 15;
    [SerializeField]
    GameObject RoadBlock = null;

    void Start()
    {
        if (transform.childCount > 0)
        {
            //siccome il canvas è il primo oggetto lo possiamo ricercare con questo comando
            canvas = transform.GetChild(0).gameObject;
            //cerchiamo all'interno del canvas il testo
            var t = canvas.GetComponentInChildren<UnityEngine.UI.Text>();
            if (t != null) 
            {
                t.text = Import.ToString();
            }
            if (type == RequestType.item) 
            {
                t.enabled = false; //disattiva il testo
                var icon = canvas.transform.Find("icon"); //cerca l'icona
                icon.GetComponent<UnityEngine.UI.Image>().sprite = requestedItem.image; //da all'icona l'immagine del requestedItem
            }
        }
        else
        {
            Debug.LogError("Canvas non trovato, assicurati che l'oggetto abbia almeno un figlio");
        }

    }

    //evento entrata
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //il canvas verrà attivato
            canvas.SetActive(true);
        }

    }

    //evento uscita
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //il canvas verrà disattivato
            canvas.SetActive(false);
        }
    }

    public void Interact()
    {
        switch (type)
        {
            case RequestType.coin:
                //controlliamo se il giocatore possiede l'importo
                if (LevelManager.instance.coin >= Import)
                {
                    LevelManager.instance.RemoveCoin(Import); //rimuoviamo l'importo
                    RoadBlock.SetActive(false); //disabilitiamo il blocco della strada
                    canvas.SetActive(false); //disabilitiamo il messaggio
                    Destroy(this); //distrugge lo script
                }
                break;
            case RequestType.item:
                if (LevelManager.instance.CanRemoveItem(requestedItem))
                {
                    RoadBlock.SetActive(false); 
                    canvas.SetActive(false); 
                    Destroy(this); 
                }
                break;
        }

       
    }

    enum RequestType
    {
        coin,
        item
    }
}
