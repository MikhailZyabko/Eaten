using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnStartScene()
    {
        SceneManager.LoadScene(1);
    }
}
