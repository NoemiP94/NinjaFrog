using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    List<GameObject> hearts = new List<GameObject>();
    [SerializeField]
    Transform heartHolder = null;
    [SerializeField]
    GameObject pausePanel = null;
    public GameObject GameOverPanel;
    [SerializeField]
    Text coinText = null;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //se premiamo ESC
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(Player.instance.health.isDeath())return;
            //attiva la pausa
            if (pausePanel.activeInHierarchy) //se è attivo
            {
                Resume();
            }
            else
            {
                pausePanel.SetActive(true); //se non è attivo lo attiviamo
                Time.timeScale = 0; //stoppiamo il tempo
            }
        }
    }

    public void ShowHp()
    {
        for (int i = 0; i < hearts.Count; i++) 
        {
            var hp = Player.instance.health.currentHp;
            if (i + 1 <= hp)
            {
                hearts[i].SetActive(true); //attiva gli oggetti
            }
            else
            {
                hearts[i].SetActive(false); //disattiva gli oggetti
            }
        }
    }

    public void AddNewLife()
    {
        GameObject g = Instantiate(hearts[0],heartHolder); //crezione nuovo cuore
        hearts.Add(g); //aggiungiamolo alla lista dei cuori
        ShowHp(); //aggiorniamo gli hp con i currentHp
    }

    public void Quit()
    {
        Resume();
        //dobbiamo uscire dal livello
        string levelName="Map";
        if (levelName == SceneManager.GetActiveScene().name)
        {
            //esce dal gioco (torna a Map)
            Application.Quit();
        }
        else
        {
          //richiamiamo la nostra scena
          SceneManager.LoadScene(levelName);
        }
        
    }

    public void Resume()
    {
        pausePanel.SetActive(false); //disattiviamo il pannello della pausa
        Time.timeScale = 1; //riattiviamo il tempo
    }

    public void Replay()
    {
        //ricarichiamo il livello corrente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowCoinText(int val)
    {
        coinText.text = val.ToString();
    }
    
}
