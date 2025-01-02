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
    }

    private void Update()
    {
        var pos = checkBorder.position;
        var downDir = pos + lookDown;
        var right = pos + lookRight;
        Debug.DrawLine(pos, downDir);
        Debug.DrawLine(pos, right);
    }
}
