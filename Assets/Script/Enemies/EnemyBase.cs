using UnityEngine;

public class EnemyBase : MonoBehaviour, Jumpable
{
    protected Health health;
    protected Animator anim;
    [SerializeField]
    protected GameObject effect = null;
    [SerializeField]
    bool destroyMonster = true;
    public void onJumpOn()
    {
        if (health.isDeath()) return;
        health.TakeDamage();
        if(anim != null)
            anim.Play("Hit");
        //se l'avversario è morto, generiamo un effetto
        if (health.isDeath())
        {
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            if(destroyMonster==true)
                //distruggiamo l'oggetto dopo 0.5 secondi
                Destroy(gameObject, 0.5f);
        }

    }

    private void Start()
    {
        Initialize();
    }
    public virtual void Initialize()
    {
        anim = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        if (health == null)
        {
            health = GetComponentInChildren<Health>();
        }
    }
}
