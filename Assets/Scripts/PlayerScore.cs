using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerScore : NetworkBehaviour
{
    public TextMeshPro scoreText;

    NetworkVariable<int> score = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        score.OnValueChanged += OnScoreChanged;
        SetScore(score.Value);
    }

    public override void OnNetworkDespawn()
    {
        score.OnValueChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int previousValue, int newValue)
    {
        SetScore(newValue);
    }

    void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreServerRPC(int score)
    {
        AddScore(score);
    }

    public void AddScore(int score)
    {
        if (IsServer)
        {
            this.score.Value += score;
        }
        else
        {
            AddScoreServerRPC(score);
        }
    }
}
