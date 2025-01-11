using UnityEngine;

public class LevelExit : MonoBehaviour,Interactable
{
    GameObject canvas;
    [SerializeField]
    string levelName = "Map";
    
    //SALVARE IL GIOCO
    const string completedLevel = "completedLevel";
    [SerializeField]
    int LevelID = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //siccome il canvas è il primo oggetto lo possiamo ricercare con questo comando
        canvas = transform.GetChild(0).gameObject;
    }

    //evento entrata
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //il canvas verrà attivato
            canvas.SetActive(true);
        }
        
    }

    //evento uscita
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //il canvas verrà disattivato
            canvas.SetActive(false);
        }
    }

    public void Interact()
    {
        //salva il gioco
        Save();
        //carica la mappa
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    //funzione per salvare il gioco
    void Save()
    {
        if(PlayerPrefs.HasKey(completedLevel))
        {
            //se il salvataggio esiste, lo leggiamo
            int current = PlayerPrefs.GetInt(completedLevel); //leggiamo il numero corrente 
            //se il livello appena completato è inferiore al LevelID
            if (current < LevelID) 
            {
                //impostiamo il nostro salvataggio
                PlayerPrefs.SetInt(completedLevel, LevelID);
            }
        }
        else
        {
            //se il salvataggio non esiste lo creiamo
            PlayerPrefs.SetInt(completedLevel, LevelID);
        }
    }
}
