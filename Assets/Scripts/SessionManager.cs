using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class SessionManager : MonoBehaviour
{
    public GameObject playerPrefab;

    Dictionary<string, NetworkObject> players = new Dictionary<string, NetworkObject>();

    void Awake()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApproval;
    }

    private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {

        var clientId = request.ClientNetworkId;
        Debug.Log(clientId);
        var connectionData = request.Payload;

        var userName = clientId == NetworkManager.Singleton.LocalClientId ? "HOST" : System.Text.Encoding.UTF8.GetString(connectionData);

        response.Approved = userName != "reject";
        response.CreatePlayerObject = false;
        response.Pending = false;

        if (response.Approved)
            Spawn(userName, clientId);
    }

    void Spawn(string userName, ulong clientId)
    {
        GameObject player;

        if (players.TryGetValue(userName, out var obj))
        {
            if (NetworkManager.Singleton.ConnectedClientsIds.Contains(obj.OwnerClientId))
            {
                NetworkManager.Singleton.DisconnectClient(obj.OwnerClientId);
            }
            obj.ChangeOwnership(clientId);
            player = obj.gameObject;
        }
        else
        {
            player = Instantiate(playerPrefab);
            player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            players.Add(userName, player.GetComponent<NetworkObject>());
        }

        player.GetComponent<PlayerName>().Setup(userName);
    }
}
