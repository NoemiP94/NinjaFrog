using UnityEngine;

public class StaticEnemy : MonoBehaviour, Jumpable
{
    Health health;
    Animator anim;
    [SerializeField]
    GameObject effect = null;

    //PROIETTILE
    [SerializeField]
    Bullet bullet = null;
    [SerializeField]
    float spawnTime = 2.5f;
    float counter = 0;
    [SerializeField]
    Vector3 lookDir = Vector3.left;
    [SerializeField]
    Transform spawnPoint = null;

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
        anim=GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        if (health == null)
        {
            health = GetComponentInChildren<Health>();
        }
        //cerchiamo animator helper
        AnimatorHelper helper = GetComponentInChildren<AnimatorHelper>();
        if (helper != null) 
        {
            helper.callback = SpawnBullet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
            return;
        }
        //cerchiamo il giocatore
        var dir = (transform.position + lookDir);
        Debug.DrawLine(transform.position, dir, Color.blue);
        var collider = Physics2D.LinecastAll(transform.position,dir);
        //se all'interno dei collider abbiamo trovato il giocatore
        foreach(var c in collider)
        {
            if(c.transform.tag == "Player")
            {
                counter = spawnTime; //il counter sarà uguale al nostro tempo
                anim.Play("Atk");
            }
        }
    }

    //funzione per generare il proiettile
    public void SpawnBullet()
    {
        Bullet newBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        newBullet.Init(lookDir);
    }
}
