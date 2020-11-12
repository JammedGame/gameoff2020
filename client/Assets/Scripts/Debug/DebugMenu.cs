using Communication;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DebugMenu
{
    private const string DebugGameName = "debug";
    private const string DebugPlayerName = "debug";

    private readonly static List<GameSetupData> games = new List<GameSetupData>();

    private static GameSetupData LastGame => games.FindLast(g => true);
    private static GameSetupData LastStartableGame => games.FindLast(g => g.players > 0 && !g.started);
    private static GameSetupData LastJoinableGame => games.FindLast(g => g.players < MoonshotServer.PlayersPerGame && !g.started);

#if UNITY_EDITOR
    [MenuItem("Moonshot/ListGames")]
#endif
    public static void ListGames()
    {
        MoonshotServer.Instance.ListGames(new ListGamesRequest(), response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(ListGames)} : {jsonResponse}");
            games.AddRange(response.games);
        });
    }

#if UNITY_EDITOR
    [MenuItem("Moonshot/FindLastGame")]
#endif
    public static void FindLastGame()
    {
        if (LastGame == null)
        {
            Debug.LogError($"{nameof(LastGame)} == null");
            return;
        }

        MoonshotServer.Instance.FindGame(new FindGameRequest
        {
            id = LastGame.id,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(FindLastGame)} : {jsonResponse}");
        });
    }

#if UNITY_EDITOR
    [MenuItem("Moonshot/StartLastStartableGame")]
#endif
    public static void StartLastStartableGame()
    {
        if (LastStartableGame == null)
        {
            Debug.LogError($"{nameof(LastStartableGame)} == null");
            return;
        }

        MoonshotServer.Instance.StartGame(new StartGameRequest
        {
            id = LastStartableGame.id,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(LastStartableGame)} : {jsonResponse}");
            LastStartableGame.started = true;
        });
    }

#if UNITY_EDITOR
    [MenuItem("Moonshot/JoinLastJoinableGame")]
#endif
    public static void JoinLastJoinableGame()
    {
        if (LastJoinableGame == null)
        {
            Debug.LogError($"{nameof(LastJoinableGame)} == null");
            return;
        }

        MoonshotServer.Instance.JoinGame(new GameJoinRequest
        {
            id = LastJoinableGame.id,
            playerName = DebugPlayerName,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(JoinLastJoinableGame)} : {jsonResponse}");
            LastJoinableGame.players++;
            GameController.Instance.PlayerState.id = response.id;
        });
    }

#if UNITY_EDITOR
    [MenuItem("Moonshot/CreateGame")]
#endif
    public static void CreateGame()
    {
        MoonshotServer.Instance.CreateGame(new CreateGameRequest
        {
            name = DebugGameName,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(CreateGame)} : {jsonResponse}");
            games.Add(new GameSetupData
            {
                id = response.id,
                name = DebugGameName,
                started = response.started,
                players = response.players.Count,
            });
        });
    }

#if UNITY_EDITOR
    [MenuItem("Moonshot/Load Gaem Scene")]
#endif
    public static void LoadGameScene()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
