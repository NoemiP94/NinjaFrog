using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    Transform start = null, end = null; //oggetti di inizio e fine
    Vector2 startPos, endPos;  //vettori che registrano le posizioni
    Vector2 destination; //destinazione
    [SerializeField]
    float moveSpeed = 1; //velocità movimento
    [SerializeField]
    float minDist = 0.5f; //distanza minima
    void Start()
    {
        startPos = start.position;
        endPos = end.position;
        destination = startPos;

    }

    // Update is called once per frame
    void Update()
    {
        //MUOVERE LA PIATTAFORMA
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed*Time.deltaTime);
        //calcoliamo la distanza
        float distance = Vector2.Distance(transform.position, destination);
        if (distance <= minDist) 
        {
            if (destination == startPos) 
            { 
                destination = endPos;
            }
            else
            {
                destination = startPos;
            }
        }

    }
}
