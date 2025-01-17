using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField]
    float lifetime = 5;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

  
    
}
