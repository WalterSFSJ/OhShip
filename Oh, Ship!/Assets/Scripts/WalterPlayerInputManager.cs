using UnityEngine;
using UnityEngine.InputSystem;

public class WalterPlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;


    private bool wasdJoined = false;
    private bool arrowsJoined = false;

    private void Start()
    {
        var player1 = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "WASD",
                pairWithDevice: Keyboard.current);

        player1.transform.position = spawnPoints[0].position;

        wasdJoined = true;




        var player2 = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "arrows",
                //controlScheme: "Arrows",
                pairWithDevice: Keyboard.current);

        player2.transform.position = spawnPoints[1].position;
    }

    // Update is called once per frame
    
}
