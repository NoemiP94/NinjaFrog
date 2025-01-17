using UnityEngine;
using UnityEngine.UI;
public class Boss : EnemyBase
{
    [SerializeField]
    Slider hpBar = null;
    [SerializeField]
    Text enemyText = null;
    [SerializeField]
    string enemyName = "FireSkull";
    [SerializeField]
    GameObject bossBattle = null;

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
    Vector3 lookDir = new Vector3(2, 0, 0);
    bool chase = false;
    float counter = 0;
    [SerializeField]
    float restTime = 2;
    Rigidbody2D rb;

    BattlePhase phase = BattlePhase.Phase1;
    [SerializeField]
    Color red, yellow, green;
    [SerializeField]
    Image fillArea = null;

    [SerializeField]
    GameObject teleportEffect = null;
    Vector3 startPosition;

    public override void Initialize()
    {
        base.Initialize();
        startPosition = transform.position; //posizione boss a inizio battaglia
        hpBar.maxValue = health.maxHp;
        hpBar.value = health.currentHp;
        health.onTakeDamage = OnTakeDamage;
        enemyText.text = enemyName;

        start = transform.position + new Vector3(maxMoveX, 0, 0);
        end = transform.position + new Vector3(-maxMoveX, 0, 0);
        destination = start;
        GetComponent<HurtBox>().OnCollisionEvent = CollisionEvent; //collega l'evento CollisionEvent a HurtBox
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Untagged"; //rende il boss intangibile
        fillArea.color = GetColor(); //imposta colore barra hp
    }

    //funzione per aggiornare gli hp
    void OnTakeDamage()
    { 
        if (health.isDeath())
        {
            if (phase == BattlePhase.End)
            {
                hpBar.maxValue = health.maxHp;
                hpBar.value = health.currentHp;
                Destroy(bossBattle); //distrugge l'oggetto
                return;
            }
            else
            {
                phase++; //passa alla prossima fase
                gameObject.tag = "Untagged"; //reimpostiamo il tag
                health.RestoreLife(); //reimposta gli hp
                Instantiate(teleportEffect,transform.position, Quaternion.identity); //crea effetto di sparizione
                transform.position = startPosition; //porta il boss in posizione originale
            }
        }
        hpBar.maxValue = health.maxHp;
        hpBar.value = health.currentHp;
        fillArea.color = GetColor(); //imposta colore barra hp
    }



    private void Update()
    {
        switch (phase)
        {
            case BattlePhase.Phase1:
                RinoMovement();
                break;
            case BattlePhase.Phase2:
                break;
            case BattlePhase.Phase3:
                break;
            case BattlePhase.End:
                break;
        }
        

    }


    #region Rino
    void Chase()
    {
        //si muove in maniera più aggressiva verso il giocatore
        transform.position += lookDir * Time.deltaTime;
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
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
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
        Debug.DrawLine(transform.position, dir, Color.red);
        //per ogni oggetto trovato in hit
        foreach (var item in hit)
        {
            //se l'oggetto trovato è il player
            if (item.transform.tag == "Player")
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
        if (collision.gameObject.tag == "Wall")
        {
            anim.SetFloat("x", 0);
            counter = restTime;
            chase = false;
            var dir = transform.position;
            //controlliamo il numero di collisioni
            if (collision.contacts.Length > 0)
            {
                dir = collision.contacts[0].normal;
            }
            dir = transform.InverseTransformPoint(dir); //inverte la direzione
            dir.Normalize(); //normalizza la direzione
            gameObject.tag = "Enemy"; //rende il boss tangibile
            anim.SetBool("vulnerable", true); //setta la variabile
            anim.Play("HitWall");
            Debug.Log(dir);
            rb.AddForce(dir * force, ForceMode2D.Impulse); //applichiamo una forza di tipo impulso per creare un sbalzo quando tocca il muro
        }
    }

    void RinoMovement()
    {
        if (counter > 0)
        {
            //da vulnerabile a in movimento
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                gameObject.tag = "Untagged"; //rende il boss intangibile
                anim.SetBool("vulnerable", false); //setta la variabile
            }
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

    #endregion

    Color GetColor()
    {
        switch (phase)
        {
            case BattlePhase.Phase1:
                return green;
            case BattlePhase.Phase2:
                return yellow;
            case BattlePhase.Phase3:
                return red;
           
        }
        return Color.blue;
    }

    enum BattlePhase
    {
        Phase1,
        Phase2,
        Phase3,
        End
    }
}
