using UnityEngine;

public class FallingEnemy : EnemyBase
{
    [SerializeField]
    Vector3 dir = Vector3.down;
    float wait = 6;
    float counter = 0;
    bool up, down;
    Rigidbody2D rb;
    Vector3 startPosition;
    [SerializeField]
    float upSpeed = 1;
    const float minDistance = 0.25f;

    public override void Initialize()
    {
        base.Initialize();
        //cerchiamo il rigidbody
        rb= GetComponent<Rigidbody2D>();
        //punto di partenza
        startPosition = transform.position;
    }
    private void Update()
    {
        if (counter > 0)
        {
            counter-=Time.deltaTime;
            if (counter <= 0)
            {
               //fase di risalita
               if (down == true)
               {
                  down = false;
                  up = true;
                  //cambia da dynamic a kinematic
                  rb.bodyType = RigidbodyType2D.Kinematic;
                  anim.SetBool("down", false);
                }
            }
            return;
        }
        if (up == true) 
        {
            //muoviamo l'oggetto tra la propria posizione e il punto di partenza
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime*upSpeed);
            float distance = Vector2.Distance(transform.position, startPosition);
            if (distance <= minDistance)
            {
                up = false;
                counter = wait / 2;
            }
            return;
        }
        //cerchiamo l'avversario
        var collider = Physics2D.LinecastAll(transform.position, (transform.position + dir));
        foreach(var item in collider)
        {
            
            //se troviamo il giocatore entra in fase caduta
            if (item.transform.tag == "Player")
            {
                counter = wait;
                //cambia da kinematic a dynamic
                rb.bodyType = RigidbodyType2D.Dynamic;
                down = true;
                anim.SetBool("down", true);
            }
        }
    }
}
