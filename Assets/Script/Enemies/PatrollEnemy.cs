using UnityEngine;

public class PatrollEnemy : EnemyBase
{
    
    

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
    float wait = 2;
    float counter = 0;
    [SerializeField]
    bool changeState = true;

    public override void Initialize()
    {
        base.Initialize();
        dir = lookRight;
    }

    private void Update()
    {
        if ( counter>0)
        {
            counter -= Time.deltaTime;
            anim.SetFloat("x", 0); //animazione Idle
            return;
        }
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
            if (changeState)
                counter = wait; //attesa
            
            
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
                if (changeState)
                    counter = wait; //attesa
            }
        }
        transform.position += dir * speed * Time.deltaTime;
        anim.SetFloat("x", 1); 
    }
}
