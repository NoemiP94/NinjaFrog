using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance; //trasformiamo il giocatore in un'istanza
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

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 groundPos = groundCheck.position; //posizione
        Debug.DrawLine(groundPos, (groundPos+lineHeight)); //disegna la riga (posizioneFloor + (posizioneFloor + lunghezzaLinea))
        //cerchiamo il pavimento
        var floors = Physics2D.LinecastAll(groundPos, (groundPos + lineHeight), floor);
        bool ground = (floors.Length>0); //se abbiamo trovato del pavimento sotto di noi
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

    //funzione per il salto
    public void Jump(float force)
    {
        //attiviamo l'animazione
        anim.Play("PlayerJump");
        //cambiamo la velocità sull'asse Y (l'asse X rimane uguale)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    //funzione animazione prendere danno
    public void TakeDamage()
    {
        anim.Play("PlayerHit");
        UIManager.instance.ShowHp();

    }
}
