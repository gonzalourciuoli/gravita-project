using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

public class SatelliteFactory : MonoBehaviour
{
    static int lineIndex = 0;
    public static void CreateSatellite(string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            if (!line.Contains("UTCGregorian"))
            {
                string[] trimmedLine = Regex.Replace(line, @"\s+", " ").Split(' ');

                float x = float.Parse(trimmedLine[4], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float y = float.Parse(trimmedLine[5], CultureInfo.InvariantCulture.NumberFormat) / 5000000;
                float z = float.Parse(trimmedLine[6], CultureInfo.InvariantCulture.NumberFormat) / 5000000;

                if (lineIndex == 0)
                {
                    UnityEngine.Debug.Log("Prueba Line Index");

                    GameObject satellitePrefab = Resources.Load<GameObject>("magellan/maggellan_ex");

                    if (satellitePrefab != null)
                    {
                        UnityEngine.Debug.Log("COMPROBACION INDEX: " + x);
                        GameObject satelliteInstance = Instantiate(satellitePrefab, new Vector3(x, y, z), Quaternion.Euler(90.0f, 0, 0));
                    }
                    else
                    {
                        UnityEngine.Debug.Log("NULL PREFAB");
                    }
                }

                lineIndex++;
            }
            UnityEngine.Debug.Log("Line Index: " + lineIndex);
        }
    }
}
