using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Vector2 checkpointPosition;
    public static LevelManager instance;
    [HideInInspector]
    public bool respawning = false;
    [SerializeField]
    CameraManager cameraMan = null;
    [SerializeField]
    List<CheckPoint> checkPointList = new List<CheckPoint>();
    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        checkpointPosition = Player.instance.transform.position; //posizione del checkpoint uguale a quella del giocatore
    }
    //funzione per impostare il checkpoint
    public void SetCheckPoint(CheckPoint c)
    {
        //disattiviamo tutti i checkpoint
        foreach(var item in checkPointList)
        {
            item.Disable();
        }
        //attiviamo quello desiderato
        c.active = true;
        //impostiamo checkpoint position
        Vector2 pos = c.transform.position;
        checkpointPosition = pos + Vector2.up;
    }
    public void Respawn()
    {
        respawning = true;
        StartCoroutine(RespawnCo());
    }

    //coroutine => permette di eseguire operazioni nel tempo, senza bloccare il thread principale del gioco
    //IEnumerator -> interfaccia necessaria per creare una coroutine
    IEnumerator RespawnCo()
    {
        cameraMan.follow = false; //disattiva la camera
        Player.instance.Deactivate(); //disattiva il player
        Player.instance.transform.position = checkpointPosition; //spostiamo il player nella posizione del checkpoint
        //comando che sospende l'esecuzione della coroutine, che attenderà prima di procedere all'istruzione successiva
        yield return new WaitForSeconds(1.7f);

        Player.instance.Activate(); //riattiva il player
        respawning=false; //ritorna a false
        cameraMan.follow = true; //riattiva la camera
    }
}
