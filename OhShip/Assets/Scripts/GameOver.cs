using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
