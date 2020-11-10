using System;
using System.Collections;
using System.Collections.Generic;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;

namespace Communication
{
    public class MoonshotServer : MonoBehaviour, IServer
    {
        public event Action<GlobalState> OnAuthoritativeStateRecieved;

        private const string ApiUrlBase = "http://localhost:8080/";
        private const string ApiUrlWebsocket = "ws://localhost:8080";

        public static MoonshotServer Instance { get; private set; }

        private WebSocket websocket;

        public bool Started { get; private set; }

        private void Start()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (websocket?.State != WebSocketState.Open) return;

            #if !UNITY_WEBGL || UNITY_EDITOR
                websocket.DispatchMessageQueue();
            #endif
        }

        private async void OnApplicationQuit()
        {
            if (websocket?.State != WebSocketState.Open) return;

            await websocket.Close();
        }

        public void ListGames(ListGamesRequest request, Action<ListGamesResponse> onResponse)
        {
            var url = $"{ApiUrlBase}game/all";
            Action<ResponseStatus, string> callback = (responseStatus, jsonResponse) =>
            {
                var response = JsonUtility.FromJson<ListGamesResponse>(jsonResponse);
                response.status = responseStatus;
                onResponse?.Invoke(response);
            };
            StartCoroutine(GetRequest(url, callback));
        }

        public void FindGame(FindGameRequest request, Action<FindGameResponse> onResponse)
        {
            var url = $"{ApiUrlBase}game?id={request.id}";
            Action<ResponseStatus, string> callback = (responseStatus, jsonResponse) =>
            {
                var response = JsonUtility.FromJson<FindGameResponse>(jsonResponse);
                response.status = responseStatus;
                onResponse?.Invoke(response);
            };
            StartCoroutine(GetRequest(url, callback));
        }

        public void StartGame(StartGameRequest request, Action<StartGameResponse> onResponse)
        {
            var url = $"{ApiUrlBase}game/start";
            var jsonRequest = JsonUtility.ToJson(request);
            Action<ResponseStatus, string> callback = (responseStatus, jsonResponse) =>
            {
                var response = JsonUtility.FromJson<StartGameResponse>(jsonResponse);
                response.status = responseStatus;
                onResponse?.Invoke(response);
            };
            StartCoroutine(PutRequest(url, jsonRequest, callback));
        }

        public void JoinGame(GameJoinRequest request, Action<GameJoinResponse> onResponse)
        {
            var url = $"{ApiUrlBase}game/join";
            var jsonRequest = JsonUtility.ToJson(request);
            Action<ResponseStatus, string> callback = (responseStatus, jsonResponse) =>
            {
                var response = JsonUtility.FromJson<GameJoinResponse>(jsonResponse);
                response.status = responseStatus;

                if (responseStatus == ResponseStatus.success) InitializeWebsocket(request.id, response.id);

                onResponse?.Invoke(response);
            };
            StartCoroutine(PutRequest(url, jsonRequest, callback));
        }

        public void CreateGame(CreateGameRequest request, Action<CreateGameResponse> onResponse)
        {
            var url = $"{ApiUrlBase}game";
            var jsonRequest = JsonUtility.ToJson(request);
            Action<ResponseStatus, string> callback = (responseStatus, jsonResponse) =>
            {
                var response = JsonUtility.FromJson<CreateGameResponse>(jsonResponse);
                response.status = responseStatus;
                onResponse?.Invoke(response);
            };
            StartCoroutine(PostRequest(url, jsonRequest, callback));
        }

        private IEnumerator GetRequest(string url, Action<ResponseStatus, string> callback)
        {
            var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            var responseStatus = request.responseCode / 100 == 2 ? ResponseStatus.success : ResponseStatus.fail;
            callback(responseStatus, request.downloadHandler.text);
        }

        private IEnumerator PutRequest(string url, string json, Action<ResponseStatus, string> callback)
        {
            var request = UnityWebRequest.Put(url, json);
            yield return request.SendWebRequest();

            var responseStatus = request.responseCode / 100 == 2 ? ResponseStatus.success : ResponseStatus.fail;
            callback(responseStatus, request.downloadHandler.text);
        }

        private IEnumerator PostRequest(string url, string json, Action<ResponseStatus, string> callback)
        {
            var request = UnityWebRequest.Post(url, json);
            yield return request.SendWebRequest();

            var responseStatus = request.responseCode / 100 == 2 ? ResponseStatus.success : ResponseStatus.fail;
            callback(responseStatus, request.downloadHandler.text);
        }

        private async void InitializeWebsocket(string gameId, string playerId)
        {
            websocket = new WebSocket(ApiUrlWebsocket, headers: new Dictionary<string, string>
            {
                [WebsocketHeaderKey.gameid.ToString()] = gameId,
                [WebsocketHeaderKey.playerid.ToString()] = playerId,
            });
            websocket.OnOpen += () => Debug.Log("Connection open!");
            websocket.OnError += e => Debug.Log("Error! " + e);
            websocket.OnClose += e => Debug.Log("Connection closed!");
            websocket.OnMessage += bytes =>
            {
                var messageData = System.Text.Encoding.UTF8.GetString(bytes);
                var message = JsonUtility.FromJson<SocketMessage>(messageData);
                if (message.type == WebsocketMessageType.start.ToString())
                {
                    Debug.Log("Game started");
                    Started = true;
                }
                else if (message.type == WebsocketMessageType.state.ToString())
                {
                    Debug.Log("Received Global State");
                    var state = message.data as GlobalState;
                    OnAuthoritativeStateRecieved?.Invoke(state);
                }
            };
            await websocket.Connect();
        }

        public async void SendClientState(PlayerState newTickState)
        {
            if (websocket?.State != WebSocketState.Open || !Started) return;

            var playerStateJson = JsonUtility.ToJson(newTickState);
            await websocket.SendText(playerStateJson);
        }
    }
}