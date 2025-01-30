using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance; //trasformiamo il giocatore in un'istanza
    [SerializeField]
    GameObject model = null; //modello del Player
    //MOVIMENTO
    [SerializeField]
    float moveSpeed = 4;
    const float multipler = 110;
    float x = 0;
    Rigidbody2D rb;//accedere al rigibody2d
    Animator anim; //aggiungiamo l'animator
    SpriteRenderer rend; //accediamo allo SpriteRenderer
    float lastX = 0; //l'ultima direzione in cui il player ha guardato

    //SALTO
    //Collisione con il pavimento
    [SerializeField]
    Transform groundCheck = null; //oggetto da cui tirare la riga
    [SerializeField]
    Vector2 lineHeight = new Vector2(0, -0.25f); //lunghezza in negativo perchè deve guardare verso il basso
    [SerializeField]
    LayerMask floor; //layer

    //Implementazione salto
    [SerializeField]
    float JumpForce = 4; //forza salto normale
    [SerializeField]
    float doubleJumpForce = 2; //forza doppio salto
    bool doubleJump = false; //doppio salto


    //SALUTE
    public Health health; //riferiment allo script Health

    //DANNI
    HitBox hitBox;

    public bool canMove = true;
    [SerializeField]
    float radius = 0.3f;
    [SerializeField]
    KeyCode interactKey = KeyCode.Z;

    //CONTRACCOLPO
    [SerializeField]
    float knockBackTime = 0.25f;
    float knockBackCounter = 0;
    Vector2 knockBackDir;
    float knockBackForce = 1;

    //SUONO
    AudioSource jumpSound;

    private void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        //rigidbody 2d si trova attaccato al Player
        //quindi bisogna richiamare il GetComponent
        rb = GetComponent<Rigidbody2D>();

        //si trova nell'oggetto sottostante a Player, ovvero NinjaFrog
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();

        health = GetComponent<Health>();
        health.onTakeDamage = TakeDamage; //impostiamo l'evento che è uguale alla funzione per l'animazione del danno

        hitBox = GetComponentInChildren<HitBox>(); 
        jumpSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (canMove == false)
        {
            return;
        }
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
            rb.linearVelocity = knockBackDir * Time.deltaTime * knockBackForce; //spostamento
            return;
        }
        Vector2 groundPos = groundCheck.position; //posizione
        Debug.DrawLine(groundPos, (groundPos+lineHeight)); //disegna la riga (posizioneFloor + (posizioneFloor + lunghezzaLinea))
        //cerchiamo il pavimento
        var floors = Physics2D.LinecastAll(groundPos, (groundPos + lineHeight), floor);
        bool ground = (floors.Length>0); //se abbiamo trovato del pavimento sotto di noi
        //impostare un parent
        if (ground == true)
        {
            if (transform.parent == null)
            {
                transform.SetParent(floors[0].transform);
            }
            //quando tocca terra disattiva l'hitbox
            SetHitBox(false);
        }
        else
        {
            transform.SetParent(null);
            //quando saltiamo attiviamo l'hitbox
            SetHitBox(true);

        }
        //la x sarà uguale all'input orizzontale
        x = Input.GetAxis("Horizontal");

        //quando premiamo il bottone "Salto"
        if (Input.GetButtonDown("Jump"))
        {
            //se stiamo toccando il pavimento
            if (ground) 
            {
                
                
                
                Jump(JumpForce);//richiamiamo la funzione di salto
            }
            else //altrimenti facciamo il doppio salto
            {
                if (!doubleJump) //se il doppio salto è false
                {
                    //diventa true
                    doubleJump = true;
                    //effettuiamo un nuovo salto
                    anim.Play("PlayerDoubleJump"); //attiviamo l'animazione doppio salto
                    //cambiamo la velocità sull'asse Y (l'asse X rimane uguale)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                }
            }
            
        }


        //moltiplicare il valore orizzontale per la velocità, per il moltiplicatore e per il tempo
        x*=moveSpeed * multipler * Time.deltaTime; //movimento costante

        //aggiungiamo la x al rigidbody
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);

        //se la x è diversa da 0
        if (x != 0) { 
            //lastX diventa la direzione in cui attualmente è girato il PG
            lastX = x;  
            //impostiamo il flip (girare il PG)
            Flip();
         }

        //ricaviamo un valore assoluto per non andare in negativo quando il PG va a SX
        var abs = Mathf.Abs(x);

        //impostiamo un valore ad anim, che è il float che
        //abbiamo impostato nei parametri dell'animator
        anim.SetFloat("x",abs);
        //impostiamo il ground nell'animator
        anim.SetBool("ground", ground);

        //se tocchiamo il pavimento, resettiamo doubleJump
        if (ground) 
        {
            doubleJump = false; 
        }
        //interazioni
        Interactions();
    }

    void Interactions()
    {
        if (Input.GetKeyDown(interactKey))
        {
            //cerca tutti i collider vicini
            var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            //per ogni oggetto trovato
            foreach (var c in colliders) 
            { 
                //controlliamo se l'oggetto contiene un'interfaccia
                Interactable interactable = c.GetComponent<Interactable>();
                //se è stato trovato
                if (interactable != null)
                {
                    //richiama la funzione interact()
                    interactable.Interact();
                    break; //interagiamo solo con il primo oggetto trovato
                }
            }
        }
    }

    //funzione per far girare il PG
    void Flip()
    {
        if (lastX < 0)
        {
            //specchia l'immagine
            rend.flipX = true;
        }
        else
        {
            //non specchia l'immagine
            rend.flipX = false;
        }
    }

    //funzione per attivare l'hitbox
    void SetHitBox(bool val)
    {
        if (hitBox == null) return; //non fa nulla
        //se non è nullo attiviamo l'oggetto
        hitBox.gameObject.SetActive(val);
    }

    //funzione per il salto
    public void Jump(float force)
    {
        if (canMove == false) return; //nessun salto
        //attiviamo l'animazione
        anim.Play("PlayerJump");
        //attiviamo il suono
        jumpSound.Play();
        //cambiamo la velocità sull'asse Y (l'asse X rimane uguale)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    //funzione animazione prendere danno
    public void TakeDamage()
    {
        if (canMove == false) return;
        anim.Play("PlayerHit");
        UIManager.instance.ShowHp();
        //controlliamo se il personaggio è morto
        if (health.isDeath())
        {
            //lo disattiviamo
            Deactivate();
            //blocchiamo la caduta del personaggio
            canMove = false;
            //impostiamo la velocità del rigidbody a 0
            rb.linearVelocity = Vector2.zero;
            //attiviamo il pannello di game over
            UIManager.instance.GameOverPanel.SetActive(true);   
        }

    }

    //funzione per il contraccolpo
    public void KnockBack(float force, Transform enemy)
    {
        if (canMove == false) return;
        knockBackCounter = knockBackTime;
        knockBackForce = force;
        var dir = transform.position - enemy.transform.position; //(posizione pg - posizione nemico)
        dir.Normalize();
        knockBackDir = dir;
    }

    //funzione per bloccare tutto
    public void Deactivate()
    {
        canMove = false;
        model.SetActive(false);
    }

    //funzione per attivare tutto
    public void Activate()
    {
        canMove = true;
        model.SetActive(true);
    }
}
