using UnityEngine;

public class PlatformOnOff : MonoBehaviour
{
    [SerializeField]
    Sprite onIMG = null, offIMG = null;
    SpriteRenderer rend;
    BoxCollider2D col;
    [SerializeField]
    bool activeOnStart = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponentInChildren<BoxCollider2D>();
        Activate(activeOnStart);
    }

    public void Activate(bool val)
    {
        if (val == true)
        {
            //se la piattaforma è attiva
            rend.sprite = onIMG;
            col.enabled = true;
        }
        else
        {
            //se la piattaforma è disattiva
            rend.sprite = offIMG;
            col.enabled = false;
        }
    }
    
}
