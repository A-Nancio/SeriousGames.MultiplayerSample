using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Text.RegularExpressions;

public class MenuControl : MonoBehaviour
{
    public TMP_InputField input;

    [SerializeField]
    private string gameSceneName = "Playground";

    public async void StartSession() 
    {
        
        // Update the current HostNameInput with whatever we have set in the NetworkConfig as default
        Debug.Log(RelayManager.Instance.IsRelayEnabled);
        if (RelayManager.Instance.IsRelayEnabled)
        {
            await RelayManager.Instance.SetupRelay();
        }
        if (NetworkManager.Singleton.StartHost())
        {
            SceneTransitionHandler.sceneTransitionHandler.RegisterCallbacks();
            SceneTransitionHandler.sceneTransitionHandler.SwitchScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    public async void JoinSession() 
    {
        if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(input.text))
        {
            await RelayManager.Instance.JoinRelay(Sanitize(input.text));
        }
        if (!NetworkManager.Singleton.StartClient())
        {
            Debug.LogError("Failed to start client.");
        }
        Debug.Log("Joining Session with code " + input.text);
    }

    public static string Sanitize(string dirtyString)
    {
        // sanitize the input for the ip address
        return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
    }
    /*
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }

        }
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    void Resume() {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }
    */
}
