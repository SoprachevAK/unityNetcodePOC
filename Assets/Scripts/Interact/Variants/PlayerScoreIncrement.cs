using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerScoreIncrement : MonoBehaviour
{
    public void OnInteractionEnd(NetworkBehaviour interactor)
    {
        Debug.Log("END");
        if (!interactor.TryGetComponent<PlayerScore>(out var playerScore)) return;
        playerScore.AddScore(1);
    }
}
