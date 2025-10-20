using UnityEngine;

public class Customization : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject cutomization;

    public void Back()
    {
        cutomization.SetActive(false);
        mainMenu.SetActive(true);
    }
}
