using UnityEngine;
using Communication;
using View;
using Logic;
using Settings;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private const float SendClientStateRate = 0.3f;

    public static GameController Instance { get; private set; }

    public CameraController cameraController;

    public List<Planet> Planets { get; } = new List<Planet>();
    private readonly List<Fighter> fighters = new List<Fighter>();
    private readonly List<WeaponProjectile> projectiles = new List<WeaponProjectile>();
    private Fighter player;
    private float timeSinceLastSendClientState = SendClientStateRate;

    public GlobalState GlobalState { get; private set; }
    public PlayerState PlayerState => player.State;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        foreach (var planetSettings in LevelId.LevelOne.GetSettings().planetSettings)
        {
            var planet = new Planet(planetSettings);
            Planets.Add(planet);
            
            var planetView = GameObject.Instantiate(Resources.Load<PlanetView>("Prefabs/PlanetView"));
            planetView.Planet = planet;
        }

        player = new Fighter(FighterType.Mosquito.GetSettings(), new PlayerState
        {
            id = "1",
            position = new Vector3(0, 0, -800f),
            rotation = Quaternion.Euler(-10, 35f, -65f),
            velocity = new Vector3(),
        });
        fighters.Add(player);

        var playerView = Instantiate(Resources.Load<FighterView>("Prefabs/FighterView"));
        playerView.Fighter = player;
        cameraController.target = playerView.transform;

        MoonshotServer.Instance.OnAuthoritativeStateRecieved += LoadAuthoritativeState;
    }

    private void LoadAuthoritativeState(GlobalState state)
    {
        if (state == null)
        {
            Debug.LogError($"{nameof(state)} == null");
            return;
        }

        if (GlobalState != null) GlobalState.CopyFrom(state, player.PlayerId);
        else GlobalState = state;
    }

    private void Update()
    {
        var dT = Time.deltaTime;

        foreach (var planet in Planets)
        {
            planet.Tick(dT);
        }

        player.SetPlayerInput(
            Quaternion.Euler(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0),
            Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward,
            Input.GetKey("space")
        );

        foreach (var fighter in fighters)
        {
            fighter.Tick(dT);
        }

        player.ClearPlayerInput();

        foreach (var projectile in projectiles)
        {
            projectile.Tick(dT);
        }

        SendClientStateIfNecessary(dT);
    }

    private void SendClientStateIfNecessary(float dT)
    {
        if (!MoonshotServer.Instance.IsStarted) return;

        timeSinceLastSendClientState += dT;
        if (timeSinceLastSendClientState > SendClientStateRate)
        {
            timeSinceLastSendClientState = 0f;
            MoonshotServer.Instance.SendClientState(player.State);
        }
    }

    private void OnDestroy()
    {
        MoonshotServer.Instance.OnAuthoritativeStateRecieved -= LoadAuthoritativeState;
    }

    public void AddProjectile(WeaponProjectile projectile)
    {
        projectiles.Add(projectile);
        
        var projectileView = Instantiate(Resources.Load<WeaponProjectileView>("Prefabs/WeaponProjectileView"));
        projectileView.WeaponProjectile = projectile;
    }
}
