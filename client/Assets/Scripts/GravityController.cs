using UnityEngine;
using System.Collections.Generic;
using Settings;

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
        AddPlanet(new Planet{position = Vector3.zero, mass = 1000000000f});
    }

    public void AddPlanet(Planet newPlanet) {
        planetList.Add(newPlanet);
    }

    public Vector3 getGravityAcceleration(Vector3 position) {
        var gravityCoefficient = GameSettings.Instance.GravityCoefficient;
        var gravityAcceleration = Vector3.zero;
        foreach (var planet in planetList)
        {
            gravityAcceleration += ((gravityCoefficient * planet.mass) / Mathf.Pow(Vector3.Distance(position, planet.position), 2)) * (planet.position - position);

        }
        return gravityAcceleration;
    }
    public class Planet {
        public Vector3 position;
        public float mass;

    }
}

