using UnityEngine;
using Settings;

public class GravityController
{
    private static GravityController instance;
    public static GravityController Instance {
        get {
            if (instance == null) {
                instance = new GravityController();
            }
            return instance;
        }
    }

    public Vector3 getGravityAcceleration(Vector3 position) {
        var gravityCoefficient = GameSettings.Instance.GravityCoefficient;
        var gravityAcceleration = Vector3.zero;
        foreach (var planet in GameController.Instance.Planets)
        {
            gravityAcceleration += ((gravityCoefficient * planet.Mass) / Mathf.Pow(Vector3.Distance(position, planet.Position), 2)) * (planet.Position - position);
        }

        return gravityAcceleration;
    }
}

