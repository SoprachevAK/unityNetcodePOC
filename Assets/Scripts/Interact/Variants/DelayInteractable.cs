using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DelayInteractable : NetworkBehaviour
{
    public float reloadInterval = 10;
    public NetworkVariable<float> reloadTime = new NetworkVariable<float>();

    Interactable interactable;
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.onInteractionEnd.AddListener(OnInteractionEnd);
    }

    public void OnInteractionEnd(NetworkBehaviour interactor)
    {
        if (IsServer) reloadTime.Value = NetworkManager.Singleton.LocalTime.TimeAsFloat + reloadInterval;
    }

    void Update()
    {
        if (IsServer)
        {
            if (reloadTime.Value < NetworkManager.Singleton.LocalTime.TimeAsFloat && interactable.state.Value == Interactable.State.Interacted)
            {
                interactable.state.Value = Interactable.State.Idle;
            }
        }
    }
}

