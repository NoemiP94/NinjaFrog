using UnityEngine;

public class FallingEnemy : EnemyBase
{
    [SerializeField]
    Vector3 dir = Vector3.down;
    float wait = 6;
    float counter = 0;
    private void Update()
    {
        //cerchiamo l'avversario
        var collider = Physics2D.LinecastAll(transform.position, (transform.position + dir));
        foreach(var item in collider)
        {
            if (counter > 0)
            {
                counter-=Time.deltaTime;
                return;
            }
            //se troviamo il giocatore entra in fase caduta
            if (item.transform.tag == "Player")
            {
                counter = wait;
            }
        }
    }
}
