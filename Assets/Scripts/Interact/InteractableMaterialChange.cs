using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InteractableMaterialChange : MonoBehaviour
{
    public Renderer renderer;
    public Material idle, interacting, interacted;

    Interactable interactable;
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.state.OnValueChanged += OnStateChanged;
    }

    private void OnStateChanged(Interactable.State previousValue, Interactable.State newValue)
    {
        switch (newValue)
        {
            case Interactable.State.Idle:
                renderer.material = idle;
                break;
            case Interactable.State.Interacting:
                renderer.material = interacting;
                break;
            case Interactable.State.Interacted:
                renderer.material = interacted;
                break;
        }
    }

}
