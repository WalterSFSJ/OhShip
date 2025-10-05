using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;
    public GameObject customization;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Customization()
    {
        mainMenu.SetActive(false);
        customization.SetActive(true);
    }
}
