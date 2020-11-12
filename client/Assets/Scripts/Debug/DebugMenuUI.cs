using UnityEngine;
using UnityEngine.UI;

public class DebugMenuUI : MonoBehaviour
{
	[SerializeField] private Button ListGames;
	[SerializeField] private Button FindLastGame;
	[SerializeField] private Button StartLastStartableGame;
	[SerializeField] private Button JoinLastJoinableGame;
	[SerializeField] private Button CreateGame;
	[SerializeField] private Button LoadGameScene;
	[SerializeField] private GameObject buttonHolder;

	private void Awake()
	{
		ListGames.onClick.AddListener(DebugMenu.ListGames);
		FindLastGame.onClick.AddListener(DebugMenu.FindLastGame);
		StartLastStartableGame.onClick.AddListener(DebugMenu.StartLastStartableGame);
		JoinLastJoinableGame.onClick.AddListener(DebugMenu.JoinLastJoinableGame);
		CreateGame.onClick.AddListener(DebugMenu.CreateGame);
		LoadGameScene.onClick.AddListener(DebugMenu.LoadGameScene);
		SetButtonsVisibility(false);
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F10))
		{
			var shouldBeShown = !ListGames.gameObject.activeInHierarchy; // toggle previous state
			SetButtonsVisibility(shouldBeShown);
		}
	}

	void SetButtonsVisibility(bool shouldBeShown)
	{
		buttonHolder.SetActive(shouldBeShown);
	}
}