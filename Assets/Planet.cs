using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Planet : Object, ICelestialBody
{
    public GameObject Initialize(string bodyName, string bodyData = null)
    {
        PlanetJson planetJson = JsonConvert.DeserializeObject<PlanetJson>(bodyData);

        GameObject planetPrefab = Resources.Load<GameObject>("Planets/Prefabs/" + bodyName);

        if (planetPrefab != null)
        {
            GameObject planetObject = Instantiate(planetPrefab, Vector3.zero, Quaternion.Euler(90.0f, 0, 0));

            PlanetInfo planetInfoComponent = planetObject.AddComponent<PlanetInfo>();
            planetInfoComponent.Initialize(planetJson);

            PlanetTrajectory trajectoryComponent = planetObject.AddComponent<PlanetTrajectory>();

            LineRenderer trajectoryRenderer = planetObject.AddComponent<LineRenderer>();
            trajectoryRenderer.enabled = false;

            float planetEquatorialRadius = planetJson.equa_radius / 10000;
            float planetPolarRadius = planetJson.polar_radius / 10000;

            planetInfoComponent.transform.localScale = new Vector3(planetEquatorialRadius, planetPolarRadius, planetEquatorialRadius);

            return planetObject;
        }
        else
        {
            UnityEngine.Debug.LogError("There was an error loading " + bodyName + "'s prefab");
            return null;
        }
    }
}
