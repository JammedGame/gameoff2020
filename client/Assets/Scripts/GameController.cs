using UnityEngine;
using Communication;
using View;
using Logic;
using Settings;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public CameraController cameraController;

    private Fighter player;
    private readonly List<Fighter> fighters = new List<Fighter>();
    private readonly List<WeaponProjectile> projectiles = new List<WeaponProjectile>();

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        player = new Fighter(FighterType.Default.GetSettings(), new PlayerState
        {
            id = 1,
            position = new Vector3(0, 0, -1200f),
            rotation = Quaternion.identity,
            velocity = new Vector3(),
        });
        fighters.Add(player);

        var playerView = Instantiate(Resources.Load<FighterView>("Prefabs/FighterView"));
        playerView.Fighter = player;
        cameraController.target = playerView.transform;
    }

    private void Update()
    {
        var dT = Time.deltaTime;

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
    }

    public void AddProjectile(WeaponProjectile projectile)
    {
        projectiles.Add(projectile);
        
        var projectileView = Instantiate(Resources.Load<WeaponProjectileView>("Prefabs/WeaponProjectileView"));
        projectileView.WeaponProjectile = projectile;
    }
}
