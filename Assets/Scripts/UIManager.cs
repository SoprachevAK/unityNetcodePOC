using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    public GameObject lobby, ingame;
    public TMP_InputField addressInput;
    public TMP_InputField nameInput;

    void Start()
    {
        ushort port = 7777;
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-port" && args.Length > i + 1)
            {
                if (ushort.TryParse(args[i + 1], out port))
                {
                    NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Port = port;
                }
            }
        }
        Debug.Log("args");
        Debug.Log(string.Join('\n', args));

#if UNITY_SERVER
        NetworkManager.Singleton.StartServer();

        Debug.Log($"____________START SERVER AT PORT {port}____________");
#endif
    }

    public void ConnectClick()
    {
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Address = addressInput.text.Split(':')[0];
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Port = ushort.Parse(addressInput.text.Split(':')[1]);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(nameInput.text);
        NetworkManager.Singleton.StartClient();

        lobby.SetActive(false);
        ingame.SetActive(true);

    }

    public void DisconnectClick()
    {
        NetworkManager.Singleton.Shutdown();
        lobby.SetActive(true);
        ingame.SetActive(false);
    }
}
