using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

public class Satellite : Object, ICelestialBody
{
    public GameObject Initialize(string bodyName, string bodyData)
    {
        int lineIndex = 0;

        GameObject satelliteObject = new GameObject();

        SatelliteTrajectory trajectoryComponent = new SatelliteTrajectory();

        List<Vector3> satellitePoints = new List<Vector3>();

        foreach (var line in File.ReadLines(bodyData))
        {
            if (!line.Contains("UTCGregorian") && lineIndex < 361)
            {
                string[] trimmedLine = Regex.Replace(line, @"\s+", " ").Split(' ');

                float x = float.Parse(trimmedLine[4], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float y = float.Parse(trimmedLine[5], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float z = float.Parse(trimmedLine[6], CultureInfo.InvariantCulture.NumberFormat) / 5000000;

                if (lineIndex == 0)
                {
                    GameObject satellitePrefab = Resources.Load<GameObject>("magellan/maggellan_ex");

                    if (satellitePrefab != null)
                    {
                        satelliteObject = Instantiate(satellitePrefab, new Vector3(x, y, z), Quaternion.Euler(90.0f, 0, 0));

                        satelliteObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        trajectoryComponent = satelliteObject.AddComponent<SatelliteTrajectory>();

                        LineRenderer trajectoryRenderer = satelliteObject.AddComponent<LineRenderer>();
                        trajectoryRenderer.enabled = true;
                    }
                    
                    else
                    {
                        UnityEngine.Debug.Log("Satellite has a null prefab");
                    }
                }

                else
                {
                    Vector3 satellitePoint = new Vector3(x, y, z);
                    satellitePoints.Add(satellitePoint);
                }

                lineIndex++;
            }
        }

        trajectoryComponent.GetTrajectories(satellitePoints);

        return satelliteObject;
    }
}