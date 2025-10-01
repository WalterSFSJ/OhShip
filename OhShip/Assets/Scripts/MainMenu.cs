using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("SergiTesting");
    }

    void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
