using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Settings;

    [System.Serializable]
    public struct Page
    {
        public string title;
        public GameObject content;
    }

    public TextMeshProUGUI titleText;
    public Page[] pages;
    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);
    }

    public void ShowPage(int index)
    {
        if (index < 0 || index >= pages.Length) return;

        // Desactivar todos
        foreach (var page in pages)
            page.content.SetActive(false);

        // Activar la seleccionada
        pages[index].content.SetActive(true);
        titleText.text = pages[index].title;
        currentPage = index;
    }

    public void NextPage()
    {
        int next = (currentPage + 1) % pages.Length;
        ShowPage(next);
    }

    public void PreviousPage()
    {
        int prev = (currentPage - 1 + pages.Length) % pages.Length;
        ShowPage(prev);
    }

    public void Return()
    {
        Settings.SetActive(false);
        MainMenu.SetActive(true);
    }
}

