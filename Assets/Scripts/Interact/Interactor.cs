using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Interactor : NetworkBehaviour
{

    Interactable interactable;

    float interactionTimer = 0;
    bool isInteracting = false;

    public void InteractButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed && interactable)
        {
            Debug.Log("InteractButtonPress");
            interactable.BeginInteractionServerRPC(this);
            interactionTimer = 3;
            isInteracting = true;
        }

        if (context.canceled)
        {
            interactable?.CancelInteractionServerRPC();
            isInteracting = false;
        }
    }

    void Update()
    {
        if (isInteracting && interactable && interactable.interactor.Value == this)
        {
            interactionTimer -= Time.deltaTime;

            if (interactionTimer < 0)
            {
                interactable.EndInteractionServerRPC();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out var interactable)) return;


        this.interactable = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out var interactable)) return;

        if (this.interactable == interactable)
        {
            if (isInteracting)
            {
                interactable.CancelInteractionServerRPC();
            }
            this.interactable = null;
        }
    }
}
