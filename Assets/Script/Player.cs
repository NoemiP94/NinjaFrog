using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4;
    const float multipler = 110;
    float x = 0;
    Rigidbody2D rb;//accedere al rigibody2d
    Animator anim; //aggiungiamo l'animator
    SpriteRenderer rend; //accediamo allo SpriteRenderer
    float lastX = 0; //l'ultima direzione in cui il player ha guardato
    void Start()
    {
        //rigidbody 2d si trova attaccato al Player
        //quindi bisogna richiamare il GetComponent
        rb = GetComponent<Rigidbody2D>();

        //si trova nell'oggetto sottostante a Player, ovvero NinjaFrog
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //la x sarà uguale all'input orizzontale
        x = Input.GetAxis("Horizontal");
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
}
