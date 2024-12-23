using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEntry : MonoBehaviour, Interactable
{
    [SerializeField]
    int LevelId = 1;
    public void Interact()
    {
       SceneManager.LoadScene(LevelId);
    }
}
