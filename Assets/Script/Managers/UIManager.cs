using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    List<GameObject> hearts = new List<GameObject>();
    [SerializeField]
    Transform heartHolder = null;

    private void Awake()
    {
        instance = this;
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
    
}
