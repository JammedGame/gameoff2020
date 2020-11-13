using System.Collections;
using System.Collections.Generic;
using Communication;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public GameObject gameItemContainer;
    private readonly static List<GameSetupData> games = new List<GameSetupData>();
    private List<LobbyGameItem> gameItems = new List<LobbyGameItem>();

    // Start is called before the first frame update
    void Start()
    {
        ListGames();
    }

    public void CreateGame(string gameName = "dusantest")
    {
        // create game
        MoonshotServer.Instance.CreateGame(new CreateGameRequest
        {
            name = gameName,
        }, response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            Debug.Log($"{nameof(CreateGame)} : {jsonResponse}");
        });
    }

    public void ListGames() 
    {
        MoonshotServer.Instance.ListGames(new ListGamesRequest(), response =>
        {
            var jsonResponse = JsonUtility.ToJson(response, true);
            // Debug.Log($"{nameof(ListGames)} : {jsonResponse}");
            games.AddRange(response.games);
            int i = 0;
            int x = 50;
            int y = 50;
            games.ForEach((game) => {
                var gameItem = Instantiate(Resources.Load<LobbyGameItem>("Prefabs/LobbyGameItem"), gameItemContainer.transform);
                gameItem.SetGameName(game.name);
                gameItems.Add(gameItem);
                gameItem.transform.localPosition = new Vector3(0, y + y * i, 0);
            });
        });

    }

    public void ClearList()
    {
    }

    public void OnEnter()
    {
        ClearList();
    }
}
