using UnityEngine;

public class DeathZone : MonoBehaviour
{
    Transform player;
    void Start()
    {
        player=Player.instance.transform;
    }

    private void Update()
    {
        if (player == null) return;
        if(player.position.y < transform.position.y)
        {
            if (LevelManager.instance.respawning == true) return; //se è già successo esce dall'if
            Player.instance.health.TakeDamage(); //danno al giocatore
            if (Player.instance.health.isDeath()) return; //se il personaggio è morto effettuiamo un ritorno
            LevelManager.instance.Respawn(); //richiama il metodo Respawn()
        }

    }
}
