using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform player;
    public bool follow = true;
    void Start()
    {
        player = Player.instance.transform;
    }

    // LateUpdate rallenta l'update, in modo che avvenga dopo il movimento del player
    void LateUpdate()
    {
        //se il player è null o se follow è false
        if (player == null || follow == false) return;
        //impostiamo la posizione uguale a quella del player
        transform.position = player.position;

    }
}
