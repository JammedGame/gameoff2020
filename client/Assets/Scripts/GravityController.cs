using UnityEngine;
using System.Collections.Generic;

public class GravityController {

    public List<Planet> planetArray;
    public float gravityCoeficient;
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
        planetArray.Add(newPlanet);
    }

    public Vector3 getGravityAcceleration(Vector3 position) {
        var gravityAcceleration = Vector3.zero;
        foreach (var planet in planetArray)
        {
            gravityAcceleration += ((gravityCoeficient * planet.mass) / Mathf.Pow(Vector3.Distance(position, planet.position), 2)) * (planet.position - position);

        }
        return gravityAcceleration;
    }
    public class Planet {
        public Vector3 position;
        public float mass;

    }
}

