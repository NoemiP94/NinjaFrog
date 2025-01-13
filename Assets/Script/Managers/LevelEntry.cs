using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEntry : MonoBehaviour, Interactable
{
    [SerializeField]
    public int LevelId = 1;
    public void Interact()
    {
        if(LevelId >= 0 && LevelId < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(LevelId);
        }
        else
        {
            Debug.LogError("LevelId non valido: " +  LevelId);
        }

       
    }
}
