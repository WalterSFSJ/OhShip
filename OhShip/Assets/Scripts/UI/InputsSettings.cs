using UnityEngine;

public class InputsSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject keyboardMenu;
    public GameObject controllerMenu;
    public void KeyboardSettings()
    {
        settingsMenu.SetActive(false);
        keyboardMenu.SetActive(true);
    }

    public void ControllerSettings()
    {
        settingsMenu.SetActive(false);
        controllerMenu.SetActive(true);
    }

    public void Back()
    {
        controllerMenu.SetActive(false);
        keyboardMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
