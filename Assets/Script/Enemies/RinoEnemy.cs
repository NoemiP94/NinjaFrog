using UnityEngine;

public class RinoEnemy : EnemyBase
{
    [SerializeField]
    float force = 1;
    [SerializeField]
    float maxMoveX = 3;
    Vector3 start, end;
    Vector3 destination;
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    float minDistance = 0.15f;
    [SerializeField]
    Vector3 lookDir = new Vector3(2,0,0);
    bool chase = false;
    float counter = 0;
    [SerializeField]
    float restTime = 2;
    Rigidbody2D rb;

    public override void Initialize()
    {
        base.Initialize();
        start = transform.position+ new Vector3(maxMoveX,0,0);
        end = transform.position + new Vector3(-maxMoveX, 0, 0);
        destination = start;
        GetComponent<HurtBox>().OnCollisionEvent = CollisionEvent; //collega l'evento CollisionEvent a HurtBox
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (counter>0)
        {
            counter-=Time.deltaTime;
            return;
        }
        if (chase)
        {
            Chase();
        }
        else
        {
            Patroll();
        }
       
    }

    void Chase()
    {
        //si muove in maniera più aggressiva verso il giocatore
        transform.position += lookDir*Time.deltaTime;
    }

    void Patroll()
    {
        if (FoundPlayer())
        {
            chase = true;
            return;
        }
        float velocity = 0.5f;
        //muoviamo l'oggetto dalla sua posizione alla sua destinazione
        transform.position = Vector3.MoveTowards(transform.position,destination, moveSpeed*Time.deltaTime);
        float dist = Vector3.Distance(transform.position, destination);
        if (dist <= minDistance)
        {
            lookDir *= -1;
            velocity = 0;
            if (destination == start)
            {
                destination = end;
            }
            else
            {
                destination = start;
            }
            //se la destinazione è < della posizione del giocatore
            if (destination.x < transform.position.x) 
            {
                //gira l'oggetto
                transform.localScale = new Vector3(-1, 1, 1);   
            }
            else 
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        anim.SetFloat("x", velocity);
    }

    //carca il giocatore
    bool FoundPlayer()
    {
        var dir = transform.position + lookDir;
        //carca tutte le hit
        var hit = Physics2D.LinecastAll(transform.position, dir);
        Debug.DrawLine(transform.position,dir,Color.red);
        //per ogni oggetto trovato in hit
        foreach(var item in hit)
        {
            //se l'oggetto trovato è il player
            if(item.transform.tag == "Player")
            {
                anim.SetFloat("x", 1);
                return true;
            }
        }
        return false;
    }

    void CollisionEvent(Collision2D collision)
    {
        //se l'ggetto che entra in collisione è il Player
        if (collision.gameObject.tag == "Player")
        {
            Health h = collision.gameObject.GetComponent<Health>(); //cerchiamo lo script della salute
            if (h != null) //se la salute non è null
            {
                h.TakeDamage();
                Player.instance.KnockBack(force, transform);
            }
        }
        if(collision.gameObject.tag == "Wall")
        {
            anim.SetFloat("x", 0);
            counter = restTime;
            chase = false;
            var dir = transform.position;
            //controlliamo il numero di collisioni
            if (collision.contacts.Length > 0) 
            {
                dir=collision.contacts[0].normal;
            }
            dir = transform.InverseTransformPoint(dir); //inverte la direzione
            dir.Normalize(); //normalizza la direzione
            anim.Play("HitWall");
            Debug.Log(dir);
            rb.AddForce(dir*force,ForceMode2D.Impulse); //applichiamo una forza di tipo impulso per creare un sbalzo quando tocca il muro
        }
    }
}
