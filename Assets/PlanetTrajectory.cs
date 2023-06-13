using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlanetTrajectory : MonoBehaviour
{
    DatabaseReference reference;
    private string bodyName;
    List<Position> trajectories;
    private List<Position> lista;
    private Coroutine movingCoroutine;
    int positionIndex = 0;

    public float transitionDuration = 1f; // Duración de la transición entre posiciones
    private bool isTransitioning = false; // Bandera para indicar si el objeto está en transición


    // Start is called before the first frame update
    void Start()
    {
        bodyName = gameObject.name.Replace("(Clone)", "");
        
        this.GetTrajectories();
    }

    void GetTrajectories()
    {
        UnityEngine.Debug.Log("-------------------------------------COMIENZA TRAJECTORY-------------------------------------");
        // Obtenemos la referencia de la trayectoria del cuerpo actual
        FirebaseDatabase.DefaultInstance.GetReference("TRAJECTORIES").Child(bodyName).GetValueAsync().ContinueWithOnMainThread(trajectoryTask =>
        {
            if (trajectoryTask.IsFaulted)
            {
                UnityEngine.Debug.Log("There was an error getting a reference for TRAJECTORIES");
            }
            else if (trajectoryTask.IsCompleted)
            {
                // Obtenemos la trayectoria del cuerpo actual
                DataSnapshot trajectorySnapshot = trajectoryTask.Result;
                string trajectoryData = trajectorySnapshot.GetRawJsonValue();

                // Guardamos todos los datos de la trayectoria del cuerpo actual
                trajectories = JsonConvert.DeserializeObject<List<Position>>(trajectoryData);

                // Obtenemos el componente LineRenderer del cuerpo
                LineRenderer lineRenderer = GetComponent<LineRenderer>();

                // Definimos una lista para guardar los puntos de la órbita que definirá el LineRenderer
                List<Vector3> orbitPoints = new List<Vector3>();

                // Iteramos sobre todas las posiciones de la trayectoria para añadirlas a la lista que utilizará el LineRenderer
                foreach (Position position in trajectories)
                {
                    // Con la posición actual, la escalamos y definimos el punto de la órbita
                    Vector3 orbitPoint = new Vector3(position.x / 5000000, position.y / 5000000, position.z / 5000000);
                    // Añadimos el punto a la lista de puntos orbitales
                    orbitPoints.Add(orbitPoint);
                }

                // Definimos la cantidad de puntos que tendrá el LineRenderer
                lineRenderer.positionCount = orbitPoints.Count;
                // Definimos la posición de los puntos del LineRenderer
                lineRenderer.SetPositions(orbitPoints.ToArray());
                // Generamos Lighting Data
                lineRenderer.generateLightingData = true;

                // Definimos los parámetros del LineRenderer
                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.3f;

                Material bodyMaterial = GameObject.Find(bodyName).GetComponent<MeshRenderer>().material;

                lineRenderer.material.color = bodyMaterial.color;

                // Llamamos al método MovePlanet con una corutina, para así actualizar la posición del cuerpo cada segundo
                movingCoroutine = StartCoroutine(MovePlanet());
            }
        });
    }


    // MovePlanet is called once per second
    IEnumerator MovePlanet()
    {
        UnityEngine.Debug.Log("COROUTINE STARTS");
        while (trajectories != null && positionIndex <= 359) // El valor máximo se cambia a 359 para evitar exceder los límites del array
        {
            if (positionIndex == 0)
            {
                UnityEngine.Debug.Log(bodyName + " First Position: " + trajectories[positionIndex].x);
            }
            // Obtenemos la posición actual del cuerpo
            Vector3 startPosition = new Vector3(trajectories[positionIndex].x / 5000000, trajectories[positionIndex].y / 5000000, trajectories[positionIndex].z / 5000000);
            // Definimos la siguiente posición del cuerpo
            Vector3 endPosition = new Vector3(trajectories[positionIndex + 1].x / 5000000, trajectories[positionIndex + 1].y / 5000000, trajectories[positionIndex + 1].z / 5000000);

            float t = 0;
            isTransitioning = true;
            while (t < 1)
            {
                t += Time.deltaTime / transitionDuration;
                // Actualizamos la posición del cuerpo mediante Lerp, consiguiendo así un efecto suave del movimiento
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            isTransitioning = false;
            positionIndex += 1;

            if (positionIndex == 360)
            {
                positionIndex = 0;
            }
        }
    }

    public void UpdateTrajectory()
    {
        StopCoroutine(movingCoroutine);
        positionIndex = 0;
        GetTrajectories();
    }

    // Update is called once per frame
    /* void Update()
    {
        if (trajectories != null && positionIndex <= 359)
        {
            float x = trajectories[positionIndex].x / 5000000;
            float y = trajectories[positionIndex].y / 5000000;
            float z = trajectories[positionIndex].z / 5000000;

            transform.position = new Vector3(x, y, z);

            positionIndex += 1;
        }
    } */
}
