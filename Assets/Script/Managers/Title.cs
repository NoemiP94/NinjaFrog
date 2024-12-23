using UnityEngine;

public class Title : MonoBehaviour
{
   public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}
