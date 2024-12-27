using UnityEngine;

public class StaticEnemy : MonoBehaviour, Jumpable
{
    Health health;
    Animator anim;
    [SerializeField]
    GameObject effect = null;
    public void onJumpOn()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
