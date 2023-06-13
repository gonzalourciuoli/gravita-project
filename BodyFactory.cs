using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class BodyFactory: MonoBehaviour
{
    public static GameObject CreateBody(string bodyName, string bodyData)
    {
        PlanetInfo planetInfo = JsonConvert.DeserializeObject<PlanetInfo>(bodyData);
        GameObject bodyPrefab = Resources.Load<GameObject>("Planets/Prefabs/" + bodyName);

        if (bodyPrefab != null)
        {
            GameObject bodyInstance = Instantiate(bodyPrefab, Vector3.zero, Quaternion.Euler(90.0f, 0, 0));
            Planet planetComponent = bodyInstance.AddComponent<Planet>();
            Trajectory trajectoryComponent = bodyInstance.AddComponent<Trajectory>();
            LineRenderer orbitTrajectory = bodyInstance.AddComponent<LineRenderer>();
            orbitTrajectory.enabled = false;

            planetComponent.Initialize(planetInfo);

            float bodyEquatorialRadius = planetInfo.equa_radius / 100000;
            float bodyPolarRadius = planetInfo.polar_radius / 100000;
            planetComponent.transform.localScale = new Vector3(bodyEquatorialRadius * 10, bodyPolarRadius * 10, bodyEquatorialRadius * 10);

            return bodyInstance;
        }
        else
        {
            UnityEngine.Debug.LogError("There was an error loading " + bodyName + "'s prefab");
            return null;
        }
    }

    public static void SetInitialPosition(string bodyName, string trajectoryData)
    {
        List<Position> trajectories = JsonConvert.DeserializeObject<List<Position>>(trajectoryData);

        if (trajectories != null && trajectories.Count > 0)
        {
            Position position = trajectories[0];
            float x = position.x / 5000000;
            float y = position.y / 5000000;
            float z = position.z / 5000000;

            GameObject bodyInstance = GameObject.Find(bodyName + "(Clone)");
            if (bodyInstance != null)
            {
                Planet planetComponent = bodyInstance.GetComponent<Planet>();
                planetComponent.transform.position = new Vector3(x, y, z);
            }
            else
            {
                UnityEngine.Debug.LogError("Could not find " + bodyName + "(Clone)");
            }
        }
    }
}

