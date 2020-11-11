using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.Networking;

namespace Communication
{
    public class MoonshotServer : MonoBehaviour, IServer
    {
        public event Action<GlobalState> OnAuthoritativeStateRecieved;

        // TODO: get this from server
        public const int PlayersPerGame = 4;
        private const string ApiUrlBase = "http://localhost:8080/";
        private const string ApiUrlWebsocket = "ws://localhost:8080";

        public static MoonshotServer Instance { get; private set; }

        private WebSocket websocket;

        public bool IsStarted { get; private set; }

        private void Start()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
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
            var responseText = request.downloadHandler.text;
            // Debug.Log($"{nameof(GetRequest)} : {responseText}");
            callback(responseStatus, responseText);
        }

        private IEnumerator PutRequest(string url, string json, Action<ResponseStatus, string> callback)
        {
            // Debug.Log($"PUT {url} : {json}");

            var request = UnityWebRequest.Put(url, json);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var responseStatus = request.responseCode / 100 == 2 ? ResponseStatus.success : ResponseStatus.fail;
            var responseText = request.downloadHandler.text;
            // Debug.Log($"{nameof(PutRequest)} : {responseText}");
            callback(responseStatus, responseText);
        }

        private IEnumerator PostRequest(string url, string json, Action<ResponseStatus, string> callback)
        {
            // Debug.Log($"POST {url} : {json}");

            var request = new UnityWebRequest(url, "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            var responseStatus = request.responseCode / 100 == 2 ? ResponseStatus.success : ResponseStatus.fail;
            var responseText = request.downloadHandler.text;
            // Debug.Log($"{nameof(PostRequest)} : {responseText}");
            callback(responseStatus, responseText);
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
            websocket.OnMessage += WebsocketOnMessage;
            await websocket.Connect();
        }

        private void WebsocketOnMessage(byte[] bytes)
        {
            var messageData = System.Text.Encoding.UTF8.GetString(bytes);
            var message = JsonUtility.FromJson<SocketMessage>(messageData);
            if (message.type == WebsocketMessageType.start.ToString())
            {
                Debug.Log($"Game started : {messageData}");
                IsStarted = true;
            }
            else if (message.type == WebsocketMessageType.state.ToString())
            {
                Debug.Log($"Received Global State : {messageData}");
                var stateMessage = JsonUtility.FromJson<StateSocketMessage>(messageData);
                var state = stateMessage.data;
                OnAuthoritativeStateRecieved?.Invoke(state);
            }
        }

        public async void SendClientState(PlayerState newTickState)
        {
            if (websocket?.State != WebSocketState.Open || !IsStarted) return;

            var playerStateJson = JsonUtility.ToJson(newTickState);
            await websocket.SendText(playerStateJson);
        }
    }
}