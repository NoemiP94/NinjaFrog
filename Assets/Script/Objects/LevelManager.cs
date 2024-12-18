using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Vector2 checkpointPosition;
    public static LevelManager instance;
    [HideInInspector]
    public bool respawning = false;
    [SerializeField]
    CameraManager cameraMan = null;
    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        checkpointPosition = Player.instance.transform.position; //posizione del checkpoint uguale a quella del giocatore
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
        //comando che sospende l'esecuzione della coroutine, che attender� prima di procedere all'istruzione successiva
        yield return new WaitForSeconds(1.7f);

        Player.instance.Activate(); //riattiva il player
        respawning=false; //ritorna a false
        cameraMan.follow = true; //riattiva la camera
    }
}
