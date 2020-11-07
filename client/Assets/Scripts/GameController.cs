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
            playerId = 1,
            x = 0, y = 0, z = -1200f,
            vx = 0, vy = 0, vz = 0,
        };
        cameraController.target = playerView.transform;
    }
}
