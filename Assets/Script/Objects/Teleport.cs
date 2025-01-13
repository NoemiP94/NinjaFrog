using UnityEngine;

public class Teleport : MonoBehaviour,Interactable
{
    GameObject canvas;
    [SerializeField]
    Teleport destination = null;
    public Transform spawnPoint = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //siccome il canvas è il primo oggetto lo possiamo ricercare con questo comando
        canvas = transform.GetChild(0).gameObject;
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
        if (destination == null) return;
        //muoviamo il giocatore
        Player.instance.transform.position = destination.spawnPoint.position;
    }
}
