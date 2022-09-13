using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PauseController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(GameIsPaused) 
            {
                pauseScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                GameIsPaused = false;
            }
            else 
            {
                pauseScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                GameIsPaused = true;
            }
        }
    }

    public void ExitGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneTransitionHandler.sceneTransitionHandler.ExitAndLoadStartMenu();
    }
}
