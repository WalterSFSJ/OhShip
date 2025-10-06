using UnityEngine;

public class Handicap_Rain : MonoBehaviour
{
    [SerializeField] Interactable puddle;

    void CauseHandycap() { 
        Interactable.Instantiate(puddle);
        
    }
}
