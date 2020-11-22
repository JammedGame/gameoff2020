using UnityEngine;
using UnityEngine.UI;

public class DebugMenuUI : MonoBehaviour
{
	private static DebugMenuUI instance;

	public Button ListGames;
	public Button FindLastGame;
	public Button StartLastStartableGame;
	public Button JoinLastJoinableGame;
	public Button CreateGame;
	public Button LoadGameScene;
	public GameObject buttonHolder;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
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