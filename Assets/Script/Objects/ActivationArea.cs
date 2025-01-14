using UnityEngine;

public class ActivationArea : MonoBehaviour
{
    [SerializeField]
    bool destroyAfterUse = true;
    [SerializeField]
    GameObject[] toActivateObj = null;
    [SerializeField]
    GameObject[] toDeactivateObj = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            foreach(var a in toActivateObj)
            {
                a.SetActive(true);
            }
            foreach (var d in toDeactivateObj)
            {
                d.SetActive(false);
            }
            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
