using UnityEngine;

public class Map : MonoBehaviour
{
    //ENTRATE
    [SerializeField]
    LevelEntry[] doors = null;
    const string completedLevel = "completedLevel";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //disattiviamo tutte le porte
        foreach(var items in doors)
        {
            items.gameObject.SetActive(false);
            int current = 0;
            //se il nostro salvataggio esiste
            if (PlayerPrefs.HasKey(completedLevel))
            {
                //il valore corrente sarà ugual a quello del nostro salvataggio
                current = PlayerPrefs.GetInt(completedLevel);
            }
            //controlliamo tutte le nostre porte
            for(int i = 0;i<doors.Length; i++)
            {
                //se la nostra porta in posizione i è <= al nostro valore corrente + 1
                if (doors[i].LevelId <= current + 1)
                {
                    //attiviamo la porta successiva al livello appena completato
                    doors[i].gameObject.SetActive(true);
                }
            }
        }

    }

    
}
