using UnityEngine;
using Communication;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        var playerView = Instantiate(Resources.Load<PlayerView>("Prefabs/PlayerView"));
        playerView.State = new PlayerState
        {
            id = 1,
            position = new Vector3(0, 0, -1200f),
            velocity = new Vector3(),
        };
        cameraController.target = playerView.transform;
    }
}
