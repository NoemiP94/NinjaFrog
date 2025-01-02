using UnityEngine;

public class PatrollEnemy : MonoBehaviour, Jumpable
{
    Health health;
    Animator anim;
    [SerializeField]
    GameObject effect = null;

    [SerializeField]
    Vector3 lookRight = Vector3.left;
    [SerializeField]
    Vector3 lookDown = Vector3.down;

    [SerializeField]
    Transform checkBorder = null;

    //MOVIMENTO
    Vector3 dir;
    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    LayerMask floor;
    [SerializeField]
    LayerMask wallLayer;

    public void onJumpOn()
    {
        if (health.isDeath()) return;
        health.TakeDamage();
        anim.Play("Hit");
        //se l'avversario è morto, generiamo un effetto
        if (health.isDeath())
        {
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            //distruggiamo l'oggetto dopo 0.5 secondi
            Destroy(gameObject, 0.5f);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        if (health == null)
        {
            health = GetComponentInChildren<Health>();
        }
        dir = lookRight;
    }

    private void Update()
    {
        var pos = checkBorder.position;
        var downDir = pos + lookDown;
        var right = pos + lookRight;
        Debug.DrawLine(pos, downDir);
        Debug.DrawLine(pos, right);
        //MOVIMENTO
        //controllo pavimento
        var collider = Physics2D.LinecastAll(checkBorder.position, downDir, floor); 
        bool ground =( collider.Length > 0); // >0 tocca il terreno
        if (!ground) //se false invertiamo la direzione
        {
            dir *= -1;

            //cambiamo direzione dell'immagine
            var scale = transform.localScale;
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
            lookRight *= -1;
        }
        else        
        {
            //controllo muro
            var wall = Physics2D.LinecastAll(checkBorder.position, right, wallLayer);
            bool hitwall = (wall.Length > 0); // >0 tocca il terreno
            if (hitwall) //se false invertiamo la direzione
            {
                dir *= -1;

                //cambiamo direzione dell'immagine
                var scale = transform.localScale;
                transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
                lookRight *= -1;
            }
        }
        transform.position += dir * speed * Time.deltaTime;
    }
}
