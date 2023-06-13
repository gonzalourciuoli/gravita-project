using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;

public class SolarSystemInitializer : MonoBehaviour
{
    // Cuerpos que queremos inicializar en la vista
    string[] bodyNames = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };

    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos la referencia de la base de datos Firebase que almacena la información de cada cuerpo y las trayectorias
        FirebaseRequests firebaseReference = FirebaseRequests.Instance;

        if (firebaseReference != null)
        {
            // Inicializamos los cuerpos deseados
            foreach (string bodyName in bodyNames)
            {
                // Para el Sol, además de inicializarlo, haremos algunos ajustes
                if (bodyName == "Sun")
                {
                    // Cargamos el prefab del Sol
                    GameObject sunPrefab = Resources.Load<GameObject>("Planets/Prefabs/Sun");

                    // Definimos el radio polar y ecuatorial del Sol
                    float solarEquatorialRadius = 6.96340f;
                    float solarPolarRadius = 6.95560f;

                    if (sunPrefab != null)
                    {
                        // Instanciamos el GameObject del Sol a partir de el prefab anteriormente obtenido y definimos su posición inicial
                        GameObject sunInstance = Instantiate(sunPrefab, Vector3.zero, Quaternion.identity);

                        // Modificamos el Transform del Sol para establecer su escala
                        sunInstance.transform.localScale = new Vector3(solarEquatorialRadius, solarPolarRadius, solarEquatorialRadius);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("There was an error loading Sun's prefab");
                    }
                }
                // Inicializamos el resto de cuerpos
                else
                {
                    // Obtenemos la referencia de la información del cuerpo actual
                    FirebaseDatabase.DefaultInstance.GetReference("BODIES").Child(bodyName).GetValueAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.IsFaulted)
                        {
                            UnityEngine.Debug.LogError("There was an error getting a reference for BODIES");
                        }
                        else if (task.IsCompleted)
                        {
                            // Obtenemos los datos del cuerpo actual
                            DataSnapshot snapshot = task.Result;
                            string bodyData = snapshot.GetRawJsonValue();

                            GameObject bodyInstance = BodyFactory.CreateBody(bodyName, bodyData);

                            // Obtenemos la referencia de la trayectoria del cuerpo actual
                            FirebaseDatabase.DefaultInstance.GetReference("TRAJECTORIES").Child(bodyName).GetValueAsync().ContinueWithOnMainThread(trajectoryTask =>
                            {
                                if (trajectoryTask.IsFaulted)
                                {
                                    UnityEngine.Debug.LogError("There was an error getting a reference for TRAJECTORIES");
                                }
                                else if (trajectoryTask.IsCompleted)
                                {
                                    // Obtenemos la trayectoria del cuerpo actual
                                    DataSnapshot trajectorySnapshot = trajectoryTask.Result;
                                    string trajectoryData = trajectorySnapshot.GetRawJsonValue();

                                    BodyFactory.SetInitialPosition(bodyName, trajectoryData);
                                }
                            });
                        }
                    });
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogError("There was a problem retrieving an instance of Firebase");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
