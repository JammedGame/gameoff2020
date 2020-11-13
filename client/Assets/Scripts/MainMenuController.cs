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

    public LobbyController lobbyController;

    void Start()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            exitButton.SetActive(false);
        #endif

        Cursor.SetCursor(Resources.Load<Texture2D>("Textures/Crosshair"), new Vector2(16f, 16f), CursorMode.Auto);
    }

    public void ClickStartGame()
    {
        mainMenu.SetActive(false);
        lobby.SetActive(true);
        lobbyController.OnEnter();
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

    public void ClickBackToMainMenu(GameObject currentItem)
    {
        currentItem.SetActive(false);
        mainMenu.SetActive(true);
    }
}
