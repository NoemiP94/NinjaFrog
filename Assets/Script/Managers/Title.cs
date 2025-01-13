using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject continueButton = null;
    private void Start()
    {
        DataCheck(); //controlliamo i dati
    }
    public void Continue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map"); //carica la mappa
    }

    public void NewGame()
    {
        DeleteData(); //cancella i dati precedenti
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map"); //carica la mappa
    }

    void DeleteData()
    {
        PlayerPrefs.DeleteAll(); //cancella tutti i dati precedenti
    }

    void DataCheck()
    {
        //controlla se il giocatore ha completato il primo livello, cioè se il primo salvataggio esiste
        if (continueButton != null) 
        { 
            if (!PlayerPrefs.HasKey("completedLevel"))
            {
                continueButton.SetActive(false); //disattiva i bottone Continua
            }
        }
        else
        {
            Debug.LogError("continueButton non è assegnato nell'Inspector.");
        }        
    }
}
