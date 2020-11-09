using Logic;
using UnityEngine;
using System.Collections.Generic;
using Settings;
using View;

public class GravityController
{
    public List<Planet> planetList = new List<Planet>();
    private static GravityController instance;
    public static GravityController Instance {
        get {
            if (instance == null) {
                instance = new GravityController();
            }
            return instance;
        }
    }
    private GravityController()
    {
        var levelSettings = LevelId.LevelOne.GetSettings();
        foreach (var planetSettings in levelSettings.planetSettings)
        {
            var planet = new Planet(planetSettings);
            AddPlanet(planet);
        }
    }

    public void AddPlanet(Planet newPlanet) {
        planetList.Add(newPlanet);
        
        var planetView = GameObject.Instantiate(Resources.Load<PlanetView>("Prefabs/PlanetView"));
        planetView.Planet = newPlanet;
    }

    public Vector3 getGravityAcceleration(Vector3 position) {
        var gravityCoefficient = GameSettings.Instance.GravityCoefficient;
        var gravityAcceleration = Vector3.zero;
        foreach (var planet in planetList)
        {
            gravityAcceleration += ((gravityCoefficient * planet.Mass) / Mathf.Pow(Vector3.Distance(position, planet.Position), 2)) * (planet.Position - position);

        }
        return gravityAcceleration;
    }
}

