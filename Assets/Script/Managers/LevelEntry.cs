using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEntry : MonoBehaviour, Interactable
{
    [SerializeField]
    public int LevelId = 1;
    public void Interact()
    {
       SceneManager.LoadScene(LevelId);
    }
}
