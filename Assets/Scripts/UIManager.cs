using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static string JoinCode = "Join Code";
    public void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static async void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            if (RelayManager.Instance.IsRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }
            NetworkManager.Singleton.StartHost();
        }
        else if (GUILayout.Button("Server")) 
        {
            if (RelayManager.Instance.IsRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }
            NetworkManager.Singleton.StartServer();
        }
        JoinCode = GUILayout.TextField(JoinCode, 10);
        if (GUILayout.Button("Client"))
        {
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(JoinCode))
            {
                await RelayManager.Instance.JoinRelay(JoinCode);
            }
            NetworkManager.Singleton.StartClient();
        }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        
        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
        if (mode == "Host" || mode == "Server")
            GUILayout.Label("Join Code: " + RelayManager.SessionCode);
    }
}