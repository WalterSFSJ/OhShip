using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public PlayerController playerController;

    private Interactable currentTarget;
    public Interactable CurrentTarget => currentTarget;

    public void SetCurrentTarget(Interactable target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        currentTarget = FindClosestInteractable();

        if (currentTarget != null
            && currentTarget.canInteract 
            && Keyboard.current.eKey.wasPressedThisFrame
            && !currentTarget.uiPanel.activeSelf)
        {
            OpenUI(currentTarget);
        }
    }

    private Interactable FindClosestInteractable()
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();
        Interactable closest = null;
        float minDist = float.MaxValue;

        foreach (var obj in interactables)
        {
            float dist = Vector3.Distance(player.position, obj.transform.position);
            if (dist < obj.interactionRange && dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private void OpenUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            Interactable[] all = FindObjectsOfType<Interactable>();
            foreach (var obj in all)
            {
                if (obj.uiPanel != null && obj != target)
                {
                    obj.uiPanel.SetActive(false);

                    var otherChar = obj.uiPanel.GetComponent<CharcoUI>();
                    if (otherChar != null) otherChar.StopMinigame();

                    var otherRed = obj.uiPanel.GetComponent<RedUI>();
                    if (otherRed != null) otherRed.StopMinigame();
                }
            }

            target.uiPanel.SetActive(true);

            var charUI = target.uiPanel.GetComponent<CharcoUI>();
            if (charUI != null)
            {
                charUI.playerInteraction = this;
                charUI.StartMinigame();
            }

            var redUI = target.uiPanel.GetComponent<RedUI>();
            if (redUI != null)
            {
                redUI.playerInteraction = this;
                redUI.StartMinigame();
            }

            playerController.enabled = false;
        }
    }

    public void CloseUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            var charUI = target.uiPanel.GetComponentInChildren<CharcoUI>();
            if (charUI != null)
            {
                charUI.StopMinigame();
                target.uiPanel.SetActive(false); 
            }

            var redUI = target.uiPanel.GetComponentInChildren<RedUI>();
            if (redUI != null)
            {
                redUI.StopMinigame();
                target.uiPanel.SetActive(false); 
            }

            target.uiPanel.SetActive(false); 
        }

        if (playerController != null)
            playerController.enabled = true;
    }
}


