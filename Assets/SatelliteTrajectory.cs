using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteTrajectory : MonoBehaviour
{
    private Coroutine movingCoroutine;
    private int positionIndex = 0;
    private float transitionDuration = 1f; // Duración de la transición entre posiciones
    public void GetTrajectories(List<Vector3> satellitePoints)
    {
        foreach (var item in satellitePoints)
        {
            UnityEngine.Debug.Log("Point: " + item);
        }
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = satellitePoints.Count;
        lineRenderer.SetPositions(satellitePoints.ToArray());
        lineRenderer.generateLightingData = true;

        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;

        /* Material bodyMaterial = GameObject.Find(bodyName).GetComponent<MeshRenderer>().material; */
        lineRenderer.material.color = Color.red;
        movingCoroutine = StartCoroutine(MovePlanet(satellitePoints));
    }

    IEnumerator MovePlanet(List<Vector3> satellitePoints)
    {
        UnityEngine.Debug.Log("COROUTINE STARTS");
        while (satellitePoints != null && positionIndex <= 359) // El valor máximo se cambia a 359 para evitar exceder los límites del array
        {
            // Obtenemos la posición actual del cuerpo
            Vector3 startPosition = new Vector3(satellitePoints[positionIndex].x, satellitePoints[positionIndex].y, satellitePoints[positionIndex].z);
            // Definimos la siguiente posición del cuerpo
            Vector3 endPosition = new Vector3(satellitePoints[positionIndex + 1].x, satellitePoints[positionIndex + 1].y, satellitePoints[positionIndex + 1].z);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / transitionDuration;
                // Actualizamos la posición del cuerpo mediante Lerp, consiguiendo así un efecto suave del movimiento
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            positionIndex += 1;

            if (positionIndex == 360)
            {
                positionIndex = 0;
            }
        }
    }
}
