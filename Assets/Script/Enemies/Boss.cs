using System.Collections;
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

    //PHASE 1
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

    //PHASE 2
    [SerializeField]
    GameObject teleportEffect = null;
    Vector3 startPosition;
    Vector3 upPosition;
    SpriteRenderer rend;
    [SerializeField]
    float attackTime = 6;
    float attackDelay = 0.6f;
    float attackCounter = 0;
    [SerializeField]
    Bullet FireProjectile = null;
    [SerializeField]
    Transform spawnPoint = null;
    bool inAction = true;

    //PHASE 3
    [SerializeField]
    PlatformOnOff[] platforms = null;
    [SerializeField]
    GameObject[] models = null;
    [SerializeField]
    GameObject[] monsters = null;

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
        rend = GetComponentInChildren<SpriteRenderer>();
        upPosition = startPosition + new Vector3(0, 1.5f, 0);
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
                StartCoroutine(TeleportCo()); //richiama la coroutine del teleport
                if (phase == BattlePhase.Phase2)
                {
                    rb.gravityScale = 0; //porta la gravità a 0
                }
                else if (phase == BattlePhase.Phase3) 
                {
                    StartCoroutine(SpawnMonsterCo());
                }

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
                Mage();
                break;
            case BattlePhase.Phase3:
                MageAndMonsters();
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

    #region Mage

    void Mage()
    {
        
        //gestione turno attacco/riposo
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            //ogni 6 secondi cambia da attacco a riposo
            inAction = !inAction;
            counter = attackTime;
            if (inAction)
            {
                //portiamo la gravità a 0
                rb.gravityScale = 0;
                gameObject.tag = "Untagged"; //rende il boss intangibile
                anim.SetBool("vulnerable", false); //diventa invulnerabile
            }
            else
            {
                //portiamo la gravità a 1
                rb.gravityScale = 1;
                gameObject.tag = "Enemy"; //rende il boss tangibile
                anim.SetBool("vulnerable", true); //setta la variabile -> diventa vulnerabile
            }
        }
        if (inAction) 
        {
            SpellAction();
        }
    }

    void SpellAction()
    {
        transform.position = Vector3.MoveTowards(transform.position, upPosition, Time.deltaTime); //porta il boss in alto
        //delay
        attackCounter -= Time.deltaTime;
        if (attackCounter <= 0)
        {
            attackCounter = attackDelay;
            var pos = new Vector3(Random.Range(-4, 6.50f), 1.15f, 0);   //imposta posizione spawnPoint
            spawnPoint.localPosition = pos; //assegna a spawnPoint la pos
            Bullet bullet = Instantiate(FireProjectile, spawnPoint.position, Quaternion.identity); //crea un proiettile
            bullet.Init(Vector3.down);
        }
    }

    void MageAndMonsters()
    {
        //gestione turno attacco/riposo
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            //ogni 6 secondi cambia da attacco a riposo
            inAction = !inAction;
            counter = attackTime;
            //attiviamo le piattaforme
            foreach(var p in platforms)
            {
                p.Activate(!inAction); //saranno attive quando non saremo in azione
            }
            if (inAction)
            {
                //portiamo la gravità a 0
                rb.gravityScale = 0;
                gameObject.tag = "Untagged"; //rende il boss intangibile
                anim.SetBool("vulnerable", false); //diventa invulnerabile
            }
            else
            {
                bool nullFounded = false;
                foreach(var m in monsters)
                {
                    if (m == null)
                    {
                        nullFounded = true;
                    }
                }
                //la coroutine parte solo se trova null
                if (nullFounded)
                {
                    StartCoroutine(SpawnMonsterCo());
                }
                
                gameObject.tag = "Enemy"; //rende il boss tangibile
                anim.SetBool("vulnerable", true); //setta la variabile -> diventa vulnerabile
            }
        }
        if (inAction)
        {
            SpellAction();
        }
    }

    //COROUTINE SPAWN MOB
    IEnumerator SpawnMonsterCo()
    {
        //controlliamo l'array dei mostri
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] == null)
            {

                //generiamo un nuovo mostro
                var m = models[i];
                Instantiate(teleportEffect, m.transform.position, m.transform.rotation);
                yield return new WaitForSeconds(0.5f); //attende 1 secondo
                GameObject newMonster = Instantiate(m, m.transform.position, m.transform.rotation);
                newMonster.SetActive(true); //se è disattivato, lo attiviamo
                monsters[i] = newMonster; //lo aggiungiamo all'array dei mostri
            }
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

    //COROUTINE TELETRASPORTO
    IEnumerator TeleportCo()
    {
        Instantiate(teleportEffect, transform.position, Quaternion.identity); //crea effetto di sparizione
        rend.enabled = false; //disattiva l'immagine
        transform.position = startPosition; //porta il boss in posizione originale
        yield return new WaitForSeconds(1); //aspetta 1 secondo
        anim.SetBool("vulnerable", false);
        health.RestoreLife(); //reimposta gli hp
        
        Instantiate(teleportEffect, startPosition, Quaternion.identity); //crea effetto di apparizione
        rend.enabled = true; //riattiva l'immagine
    }
}
