using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TwoPlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        var player1 = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "Keyboard&Mouse",
                pairWithDevice: Keyboard.current);

        player1.name = "WASD";

        player1.transform.position = spawnPoints[0].position;

        var player2 = PlayerInput.Instantiate(player2Prefab,
                controlScheme: "Arrows",
                pairWithDevice: Keyboard.current);

        player2.name = "arrows";
        
        player2.transform.position = spawnPoints[1].position;

        StartCoroutine(Reload(player1.transform));
    }

    IEnumerator Reload(Transform tr) {
        yield return new WaitForSeconds(0.05f);
        if (!(tr.position.x > -15.0f && tr.position.x < -13.0f))
            SceneManager.LoadScene(1);
    }
}

