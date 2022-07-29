using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using TMPro;
using Unity.Collections;
using System;

public class PlayerName : NetworkBehaviour
{
    public Transform hud;
    public TextMeshPro nameText;

    NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>();

    public override void OnNetworkSpawn()
    {
        playerName.OnValueChanged += OnNameChanged;
        nameText.text = playerName.Value.ToString();
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnNameChanged;
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        nameText.text = playerName.Value.ToString();
    }

    void Start()
    {
        if (IsOwner)
        {
            FindObjectOfType<CinemachineVirtualCamera>().Follow = transform;
        }

        hud.GetComponent<UnityEngine.Animations.LookAtConstraint>().AddSource(new UnityEngine.Animations.ConstraintSource()
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        });
    }


    public void Setup(string name)
    {
        playerName.Value = name;
    }
}
