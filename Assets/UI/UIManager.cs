using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        StatusLabels();
        GUILayout.EndArea();
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