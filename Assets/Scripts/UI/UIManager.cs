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
        StatusLabels();
        //if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        //{
        //    StartButtons();
        //}
        //else
        //{
        //}

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