using Communication;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DebugMenu
{
    private const string DebugGameName = "debug";
    private const string DebugPlayerName = "debug";

    private readonly static List<GameSetupData> games = new List<GameSetupData>();

    private static GameSetupData LastGame => games.FindLast(g => true);
    private static GameSetupData LastStartableGame => games.FindLast(g => g.players > 0 && !g.started);
    private static GameSetupData LastStartedGame => games.FindLast(g => g.started);
    private static GameSetupData LastJoinableGame => games.FindLast(g => g.players < MoonshotServer.PlayersPerGame && !g.started);

    [MenuItem("Moonshot/ListGames")]
    private static void ListGames()
    {
        MoonshotServer.Instance.ListGames(new ListGamesRequest(), response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(ListGames)} : {jsonResponse}");

            games.AddRange(response.games);
        });
    }

    [MenuItem("Moonshot/FindLastGame")]
    private static void FindLastGame()
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

    [MenuItem("Moonshot/StartLastStartableGame")]
    private static void StartLastStartableGame()
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

    [MenuItem("Moonshot/JoinLastJoinableGame")]
    private static void JoinLastJoinableGame()
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
        });
    }

    [MenuItem("Moonshot/CreateGame")]
    private static void CreateGame()
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
}
