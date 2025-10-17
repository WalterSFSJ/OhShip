using UnityEngine;
using UnityEngine.InputSystem;

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
    }
}

