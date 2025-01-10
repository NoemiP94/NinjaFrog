using UnityEngine;

public class Trampoline : MonoBehaviour, Jumpable
{
    [SerializeField]
    float force=10;
    public void onJumpOn()
    {
        Player.instance.Jump(force);
    }
}

