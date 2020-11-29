using UnityEngine;
using Communication;
using View;
using Logic;
using Settings;
using System.Collections.Generic;
using Audio;

public class GameController : MonoBehaviour
{
    private const float SendClientStateRate = 0.3f;

    public static GameController Instance { get; private set; }

    public GameViewController ViewController;

    public readonly List<Planet> Planets = new List<Planet>();
    public readonly List<Fighter> Fighters = new List<Fighter>();
    public readonly List<WeaponProjectile> Projectiles = new List<WeaponProjectile>();
    public readonly List<Mothership> Motherships = new List<Mothership>();
    public readonly List<Drone> Drones = new List<Drone>();

    private Fighter player;
    private float timeSinceLastSendClientState = SendClientStateRate;

    public GlobalState GlobalState { get; private set; }
    public PlayerState PlayerState => player.State;

    private const float combatEndBuffer = 5f;
    private bool inCombat;
    private float timeSinceCombatStarted;
    private float timeSinceCombatEnded;

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        var levelSettings = LevelId.LevelOne.GetSettings();
        foreach (var planetSettings in levelSettings.planetSettings)
        {
            var planet = new Planet(planetSettings);
            Planets.Add(planet);
        }

        player = new Fighter(FighterType.Mosquito.GetSettings(), new PlayerState
        {
            id = "1",
            position = new Vector3(0, 0, -800f),
            rotation = Quaternion.Euler(-10, 35f, -65f),
            velocity = new Vector3(),
        });
        Fighters.Add(player);

        foreach (var mothershipSettings in levelSettings.mothershipSettings)
        {
            var mothership = new Mothership(mothershipSettings);
            Motherships.Add(mothership);
        }

        Cursor.SetCursor(Resources.Load<Texture2D>("Textures/CrosshairMove"), new Vector2(16f, 16f), CursorMode.Auto);

        MoonshotServer.Instance.OnAuthoritativeStateRecieved += LoadAuthoritativeState;

        // todo jole
        // MoonshotAudioManager.Instance.StartMusic();
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

        var steerTarget = new Vector2(
            Mathf.Clamp(2 * Input.mousePosition.x / Screen.width - 1f, -1f, 1f) * Screen.width / Screen.height,
            Mathf.Clamp(2 * Input.mousePosition.y / Screen.height - 1f, -1f, 1f)
        );
        steerTarget = Vector2.ClampMagnitude(steerTarget, 1f);
        var shootAngle = Quaternion.identity; //steerTarget * (Camera.main.fieldOfView * 0.5f);
        var currentInput = new FighterInput
        {
            SteerTarget = steerTarget,
            Throttle = Input.GetAxis("Vertical"),
            Roll = Input.GetAxis("Horizontal"),
            ShootDirection = Quaternion.Euler(-shootAngle.y, shootAngle.x, 0),
            Shoot = Input.GetButton("Fire1"),
            Drift = Input.GetButton("Fire3"),
        };
        player.SetPlayerInput(currentInput);

        for (var i = 0; i < Projectiles.Count; i++)
        {
            var projectile = Projectiles[i];
            projectile.Tick(dT);
            if (projectile.Dead)
            {
                Projectiles.RemoveAt(i--);
                continue;
            }
        }

        for (var i = 0; i < Fighters.Count; i++)
        {
            var fighter = Fighters[i];
            fighter.Tick(dT);
            for (var j = 0; j < Projectiles.Count; j++)
            {
                var projectile = Projectiles[j];
                if (fighter.TryCollideWith(projectile)) Projectiles[j].GetKilled(fighter);
            }
            if (fighter.Dead) Fighters.RemoveAt(i--);
        }

        foreach (var mothership in Motherships)
        {
            mothership.Tick(dT);
        }

        for (var i = 0; i < Drones.Count; i++)
        {
            var drone = Drones[i];
            drone.Tick(dT);
            for (var j = 0; j < Projectiles.Count; j++)
            {
                var projectile = Projectiles[j];
                if (drone.TryCollideWith(projectile)) Projectiles[j].GetKilled(drone);
            }
            if (drone.Dead) Drones.RemoveAt(i--);
        }

        SendClientStateIfNecessary(dT);
        ViewController.UpdateViews(this);

        if (currentInput.Shoot)
        {
            timeSinceCombatStarted += dT;
            timeSinceCombatEnded = 0;
            if (!inCombat)
            {
                inCombat = true;
                MoonshotAudioManager.Instance.FadeInCombatMusic();
            }
        }
        else
        {
            timeSinceCombatEnded += dT;
            if (timeSinceCombatEnded >= combatEndBuffer && inCombat)
            {
                inCombat = false;
                MoonshotAudioManager.Instance.FadeOutCombatMusic();
            }
        }
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

    public Vector3 GetGravityAcceleration(Vector3 position)
    {
        var gravityCoefficient = GameSettings.Instance.GravityCoefficient;
        var gravityAcceleration = Vector3.zero;
        foreach (var planet in Planets)
        {
            gravityAcceleration += ((gravityCoefficient * planet.Mass) / Mathf.Pow(Vector3.Distance(position, planet.Position), 2)) * (planet.Position - position);
        }

        return gravityAcceleration;
    }

    public Planet IsInAtmosphereOfPlanet(Vector3 position)
    {
        return Planets.Find(p => Vector3.Distance(position, p.Position) <= p.RadiusIncludingAtmosphere);
    }

    public void AddProjectile(WeaponProjectile projectile)
    {
        Projectiles.Add(projectile);
    }

    public void AddDrone(Drone drone)
    {
        Drones.Add(drone);
    }

    public Fighter GetNearestTarget(Vector3 position, Team team)
    {
        var minDistance = float.MaxValue;
        var result = default(Fighter);
        foreach (var target in Fighters)
        {
            var targetDistance = Vector3.Distance(position, target.Position);
            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                result = target;
            }
        }
        return result;
    }
}
