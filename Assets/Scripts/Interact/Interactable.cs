using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class Interactable : NetworkBehaviour
{
    public enum State
    {
        Idle,
        Interacting,
        Interacted
    }

    public NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    public NetworkVariable<NetworkBehaviourReference> interactor = new NetworkVariable<NetworkBehaviourReference>();

    public UnityEvent<NetworkBehaviour> onInteractionBegin;
    public UnityEvent<NetworkBehaviour> onInteractionCancel;
    public UnityEvent<NetworkBehaviour> onInteractionEnd;

    [ServerRpc(RequireOwnership = false)]
    public void BeginInteractionServerRPC(NetworkBehaviourReference interactorRef)
    {
        if (state.Value == State.Idle)
        {
            state.Value = State.Interacting;
            interactor.Value = interactorRef;
            onInteractionBegin.Invoke(interactorRef);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CancelInteractionServerRPC()
    {
        if (state.Value == State.Interacting)
        {
            state.Value = State.Idle;
            onInteractionCancel.Invoke(interactor.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndInteractionServerRPC()
    {
        if (state.Value == State.Interacting)
        {
            state.Value = State.Interacted;
            onInteractionEnd.Invoke(interactor.Value);
        }
    }

}
