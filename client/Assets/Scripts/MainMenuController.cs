using System.Collections;
using System.Collections.Generic;
using Communication;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public enum MainMenuItem 
    {
        Main,
        Lobby,
        Options,
        Credits
    }

    public GameObject mainMenu;
    public GameObject lobby;
    public GameObject options;
    public GameObject credits;

    public GameObject exitButton;

    void Start()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            exitButton.SetActive(false);
        #endif
    }

    public void CreateGame()
    {
        var DebugGameName = "dusantest";

        // create game
        MoonshotServer.Instance.CreateGame(new CreateGameRequest
        {
            name = DebugGameName,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(CreateGame)} : {jsonResponse}");
        });
    }

    public void ClickStartGame()
    {
        mainMenu.SetActive(false);
        lobby.SetActive(true);

        CreateGame();
    }

    public void ClickOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }
    public void ClickCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }
    public void ClickExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif !UNITY_WEBGL
            Application.Quit();
        #endif
    }
}
