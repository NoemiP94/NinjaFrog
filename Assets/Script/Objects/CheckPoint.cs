using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [HideInInspector]
    public bool active = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Disable()
    {
        active = false;
        anim.Play("Close");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //se il checkpoint è disabilidato
            if (!active)
            {
                //richiama LevelManager e impostiamo il checkpoint
                LevelManager.instance.SetCheckPoint(this);
                //riprodurre animazione
                anim.Play("Open");
            }
        }
    }
}
