using System.Linq;
using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using SFB;

public class UIController : MonoBehaviour
{
    string[] bodyNames = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
    public GameObject panelSliderMenu;
    private GameObject selectedPlanet;
    public GameObject panelInfo;
    public GameObject planetNameTextObject;
    public GameObject panelTabMenu;
    public GameObject panelTrajectoryMenu;
    public Button calculateTrajectoriesButton;
    public Slider progressBar;
    public Button loadTrajectoriesButton;
    private int tabPressCount = 0;
    bool[] infoShow = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    string[] info = { "alternative_name", "aphelion", "arg_periapsis", "avg_temp", "axial_tilt", "body_id", "body_name", "body_type", "density", "dimension", "discovered_by", "discovery_date",
    "eccentricity", "equa_radius", "escape", "flattening", "gravity", "inclination", "is_planet", "long_asc_node", "main_anomaly", "mass", "mean_radius", "moons", "perihelion", "semimajor_axis",
    "sideral_orbit", "sideral_rotation", "vol" };

    // Start is called before the first frame update
    void Start()
    {
        AddPlanetNamesToMenu();
        panelInfo.SetActive(false);
        panelTabMenu.SetActive(false);

        // Encuentra el botón ShowPlanisphereButton y agrega el método OnShowPlanisphereButtonClick como listener
        Button showPlanisphereButton = GameObject.Find("ShowPlanisphereButton").GetComponent<Button>();
        showPlanisphereButton.onClick.AddListener(OnShowPlanisphereButtonClick);

        calculateTrajectoriesButton.onClick.AddListener(OnCalculateTrajectoriesButtonClick);

        loadTrajectoriesButton.onClick.AddListener(OpenFileDialog);
    }

    private void AddPlanetNamesToMenu()
    {
        foreach (string bodyName in bodyNames)
        {
            // Crea un nuevo objeto como contenedor del texto y el recuadro
            GameObject container = new GameObject(bodyName + "_Container");
            container.AddComponent<RectTransform>();

            // Configura el componente RectTransform del contenedor
            RectTransform containerRectTransform = container.GetComponent<RectTransform>();
            containerRectTransform.SetParent(panelSliderMenu.transform, false);
            containerRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            containerRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            containerRectTransform.pivot = new Vector2(0.5f, 0.5f);
            containerRectTransform.sizeDelta = new Vector2(150, 25);

            // Posiciona el contenedor en función del índice del planeta en el arreglo planetNames
            int index = Array.IndexOf(bodyNames, bodyName);
            containerRectTransform.anchoredPosition = new Vector2(0, -30 * index);

            // Agrega un componente Image al contenedor como recuadro
            Image backgroundImage = container.AddComponent<Image>();
            backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Color gris-negro

            // Crea un nuevo objeto Text como hijo del contenedor
            GameObject newText = new GameObject(bodyName + "_Text");
            newText.AddComponent<Text>();

            // Configura el componente Text
            Text textComponent = newText.GetComponent<Text>();
            textComponent.text = bodyName;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;
            textComponent.fontSize = 14;

            // Configura el componente RectTransform del texto
            RectTransform rectTransform = newText.GetComponent<RectTransform>();
            rectTransform.SetParent(container.transform, false);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);

            // Agrega un componente EventTrigger al objeto Text
            EventTrigger eventTrigger = newText.AddComponent<EventTrigger>();

            // Crea una nueva entrada para el evento PointerClick (clic del ratón)
            EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
            pointerClickEntry.eventID = EventTriggerType.PointerClick;

            // Añade un UnityAction que llama al método SelectPlanet con el nombre del planeta al hacer clic
            pointerClickEntry.callback.AddListener((eventData) => { SelectPlanet(bodyName); });

            // Agrega la entrada de evento al componente EventTrigger
            eventTrigger.triggers.Add(pointerClickEntry);
        }
    }
    public void SelectPlanet(string bodyName)
    {
        GameObject newSelectedPlanet = GameObject.Find(bodyName + "(Clone)");

        // Si se hace clic en el mismo nombre de planeta, CameraSetting.Instance.target será null
        if (selectedPlanet == newSelectedPlanet)
        {
            CameraSetting.Instance.target = null;
            selectedPlanet = null;

            // Si seleccionamos el mismo planeta que ya estabamos seleccionando, desactivamos el panel
            panelInfo.SetActive(false);
        }
        else
        {
            selectedPlanet = newSelectedPlanet;
            CameraSetting.Instance.target = selectedPlanet.transform;

            var distance = selectedPlanet.transform.localScale.x * 2.5f;
            CameraSetting.Instance.distance = distance;
            CameraSetting.Instance.minDistance = distance;

            foreach (string bodyName_ in bodyNames)
            {
                if (!bodyName_.Contains("Sun"))
                {
                    // Buscamos el cuerpo y desactivamos el dibujo de la trayectoria
                    GameObject currentBody = GameObject.Find(bodyName_ + "(Clone)");
                    currentBody.GetComponent<LineRenderer>().enabled = false;
                }
            }

            // Si seleccionamos un planeta, activamos el panel
            panelInfo.SetActive(true);

            TextMeshProUGUI planetNameText = planetNameTextObject.GetComponent<TextMeshProUGUI>();

            // Cambiamos el nombre del planeta 
            planetNameText.text = bodyName;

            Planet selectedPlanetInfo = newSelectedPlanet.GetComponent<Planet>();

            // Editamos la información del cuerpo
            GameObject alternative_name_object = GameObject.Find("alternative_name");
            TextMeshProUGUI alternative_name = alternative_name_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.alternative_name != "")
            {
                alternative_name.text = "Alternative name: " + selectedPlanetInfo.alternative_name;
            }
            else
            {
                alternative_name.enabled = false;
            }

            GameObject aphelion_object = GameObject.Find("aphelion");
            TextMeshProUGUI aphelion = aphelion_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.aphelion.ToString() != "")
            {
                aphelion.text = "Aphelion: " + selectedPlanetInfo.aphelion.ToString();
            }
            else
            {
                aphelion.enabled = false;
            }

            GameObject arg_periapsis_object = GameObject.Find("arg_periapsis");
            TextMeshProUGUI arg_periapsis = arg_periapsis_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.arg_periapsis.ToString() != "")
            {
                arg_periapsis.text = "Argument of periapsis: " + selectedPlanetInfo.arg_periapsis.ToString();
            }
            else
            {
                arg_periapsis.enabled = false;
            }

            GameObject avg_temp_object = GameObject.Find("avg_temp");
            TextMeshProUGUI avg_temp = avg_temp_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.avg_temp.ToString() != "")
            {
                avg_temp.text = "Average temperature: " + selectedPlanetInfo.avg_temp.ToString();
            }
            else
            {
                avg_temp.enabled = false;
            }

            GameObject axial_tilt_object = GameObject.Find("axial_tilt");
            TextMeshProUGUI axial_tilt = axial_tilt_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.axial_tilt.ToString() != "")
            {
                axial_tilt.text = "Axial tilt: " + selectedPlanetInfo.axial_tilt.ToString();
            }
            else
            {
                axial_tilt.enabled = false;
            }

            GameObject body_id_object = GameObject.Find("body_id");
            TextMeshProUGUI body_id = body_id_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.body_id != "")
            {
                body_id.text = "Body id: " + selectedPlanetInfo.body_id;
            }
            else
            {
                body_id.enabled = false;
            }

            GameObject body_name_object = GameObject.Find("body_name");
            TextMeshProUGUI body_name = body_name_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.body_name != "")
            {
                body_name.text = "Body name: " + selectedPlanetInfo.body_name;
            }
            else
            {
                body_name.enabled = false;
            }

            GameObject body_type_object = GameObject.Find("body_type");
            TextMeshProUGUI body_type = body_type_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.body_type != "")
            {
                body_type.text = "Body type: " + selectedPlanetInfo.body_type;
            }
            else
            {
                body_type.enabled = false;
            }

            GameObject density_object = GameObject.Find("density");
            TextMeshProUGUI density = density_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.density.ToString() != "")
            {
                density.text = "Density: " + selectedPlanetInfo.density.ToString();
            }
            else
            {
                density.enabled = false;
            }

            GameObject dimension_object = GameObject.Find("dimension");
            TextMeshProUGUI dimension = dimension_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.dimension != "")
            {
                dimension.text = "Dimension: " + selectedPlanetInfo.dimension;
            }
            else
            {
                dimension.enabled = false;
            }

            GameObject discovered_by_object = GameObject.Find("discovered_by");
            TextMeshProUGUI discovered_by = discovered_by_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.discovered_by != "")
            {
                discovered_by.text = "Discovered by: " + selectedPlanetInfo.discovered_by;
            }
            else
            {
                discovered_by.enabled = false;
            }

            GameObject discovery_date_object = GameObject.Find("discovery_date");
            TextMeshProUGUI discovery_date = discovery_date_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.discovery_date != "")
            {
                discovery_date.text = "Discovery date: " + selectedPlanetInfo.discovery_date;
            }
            else
            {
                discovery_date.enabled = false;
            }

            GameObject eccentricity_object = GameObject.Find("eccentricity");
            TextMeshProUGUI eccentricity = eccentricity_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.eccentricity.ToString() != "")
            {
                eccentricity.text = "Eccentricity: " + selectedPlanetInfo.eccentricity.ToString();
            }
            else
            {
                eccentricity.enabled = false;
            }

            GameObject equa_radius_object = GameObject.Find("equa_radius");
            TextMeshProUGUI equa_radius = equa_radius_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.equa_radius.ToString() != "")
            {
                equa_radius.text = "Equatorial radius: " + selectedPlanetInfo.equa_radius.ToString();
            }
            else
            {
                equa_radius.enabled = false;
            }

            GameObject escape_object = GameObject.Find("escape");
            TextMeshProUGUI escape = escape_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.escape.ToString() != "")
            {
                escape.text = "Escape: " + selectedPlanetInfo.escape.ToString();
            }
            else
            {
                escape.enabled = false;
            }

            GameObject flattening_object = GameObject.Find("flattening");
            TextMeshProUGUI flattening = flattening_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.flattening.ToString() != "")
            {
                flattening.text = "Flattening: " + selectedPlanetInfo.flattening.ToString();
            }
            else
            {
                flattening.enabled = false;
            }

            GameObject gravity_object = GameObject.Find("gravity");
            TextMeshProUGUI gravity = gravity_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.gravity.ToString() != "")
            {
                gravity.text = "Gravity: " + selectedPlanetInfo.gravity.ToString();
            }
            else
            {
                gravity.enabled = false;
            }

            GameObject inclination_object = GameObject.Find("inclination");
            TextMeshProUGUI inclination = inclination_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.inclination.ToString() != "")
            {
                inclination.text = "Inclination: " + selectedPlanetInfo.inclination.ToString();
            }
            else
            {
                inclination.enabled = false;
            }

            GameObject is_planet_object = GameObject.Find("is_planet");
            TextMeshProUGUI is_planet = is_planet_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.is_planet.ToString() != "")
            {
                is_planet.text = "Is planet: " + selectedPlanetInfo.is_planet.ToString();
            }
            else
            {
                is_planet.enabled = false;
            }

            GameObject long_asc_node_object = GameObject.Find("long_asc_node");
            TextMeshProUGUI long_asc_node = long_asc_node_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.long_asc_node.ToString() != "")
            {
                long_asc_node.text = "Longitude of ascending node: " + selectedPlanetInfo.long_asc_node.ToString();
            }
            else
            {
                long_asc_node.enabled = false;
            }

            GameObject main_anomaly_object = GameObject.Find("main_anomaly");
            TextMeshProUGUI main_anomaly = main_anomaly_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.main_anomaly.ToString() != "")
            {
                main_anomaly.text = "Main anomaly: " + selectedPlanetInfo.main_anomaly.ToString();
            }
            else
            {
                main_anomaly.enabled = false;
            }

            GameObject mass_object = GameObject.Find("mass");
            TextMeshProUGUI mass = mass_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.mass.ToString() != "")
            {
                mass.text = "Mass: " + selectedPlanetInfo.mass.ToString();
            }
            else
            {
                mass.enabled = false;
            }

            GameObject mean_radius_object = GameObject.Find("mean_radius");
            TextMeshProUGUI mean_radius = mean_radius_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.mean_radius.ToString() != "")
            {
                mean_radius.text = "Mean radius: " + selectedPlanetInfo.mean_radius.ToString();
            }
            else
            {
                mean_radius.enabled = false;
            }

            GameObject moons_object = GameObject.Find("moons");
            TextMeshProUGUI moons = moons_object.GetComponent<TextMeshProUGUI>();

            /* moons.text = selectedPlanetInfo.moons.ToString(); */

            GameObject perihelion_object = GameObject.Find("perihelion");
            TextMeshProUGUI perihelion = perihelion_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.perihelion.ToString() != "")
            {
                perihelion.text = "Perihelion: " + selectedPlanetInfo.perihelion.ToString();
            }
            else
            {
                perihelion.enabled = false;
            }

            GameObject semimajor_axis_object = GameObject.Find("semimajor_axis");
            TextMeshProUGUI semimajor_axis = semimajor_axis_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.semimajor_axis.ToString() != "")
            {
                semimajor_axis.text = "Semimajor axis: " + selectedPlanetInfo.semimajor_axis.ToString();
            }
            else
            {
                semimajor_axis.enabled = false;
            }

            GameObject sideral_orbit_object = GameObject.Find("sideral_orbit");
            TextMeshProUGUI sideral_orbit = sideral_orbit_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.sideral_orbit.ToString() != "")
            {
                sideral_orbit.text = "Sideral orbit: " + selectedPlanetInfo.sideral_orbit.ToString();
            }
            else
            {
                sideral_orbit.enabled = false;
            }

            GameObject sideral_rotation_object = GameObject.Find("sideral_rotation");
            TextMeshProUGUI sideral_rotation = sideral_rotation_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.sideral_rotation.ToString() != "")
            {
                sideral_rotation.text = "Sideral rotation: " + selectedPlanetInfo.sideral_rotation.ToString();
            }
            else
            {
                sideral_rotation.enabled = false;
            }

            GameObject vol_object = GameObject.Find("vol");
            TextMeshProUGUI vol = vol_object.GetComponent<TextMeshProUGUI>();

            if (selectedPlanetInfo.vol.ToString() != "")
            {
                vol.text = "Vol: " + selectedPlanetInfo.vol.ToString();
            }
            else
            {
                vol.enabled = false;
            }
        }
    }

    // Método que se llama cuando se hace click en ShowPlanisphereButton
    public void OnShowPlanisphereButtonClick()
    {
        /* CameraSetting.Instance.cam.transform.position = new Vector3(100, 100, 100); */

        foreach (string bodyName in bodyNames)
        {
            if (!bodyName.Contains("Sun"))
            {
                // Buscamos el cuerpo y activamos el dibujo de la trayectoria
                GameObject currentBody = GameObject.Find(bodyName + "(Clone)");
                currentBody.GetComponent<LineRenderer>().enabled = true;
            }
        }

        if (panelInfo.activeInHierarchy)
        {
            panelInfo.SetActive(false);
        }

        CameraSetting.Instance.target = GameObject.Find("Sun(Clone)").transform;
        CameraSetting.Instance.distance = 100.0f;
    }

    // Método que se llama cuando se hace click en CalculateTrajectoriesButton
    public void OnCalculateTrajectoriesButtonClick()
    {
        progressBar.gameObject.SetActive(!progressBar.gameObject.activeSelf);

        GameObject initial_date = GameObject.Find("initial_date");
        GameObject final_date = GameObject.Find("final_date");

        TMP_InputField initial_date_text = initial_date.GetComponent<TMP_InputField>();
        TMP_InputField final_date_text = final_date.GetComponent<TMP_InputField>();

        var url = "http://127.0.0.1:5000/gravita/trajectories";
        var queryParams = "?initial_date=" + initial_date_text.text + "&final_date=" + final_date_text.text;

        StartCoroutine(PostTrajectories(url + queryParams));
    }

    public IEnumerator PostTrajectories(string url)
    {
        var request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            foreach (string bodyName in bodyNames)
            {
                if (bodyName != "Sun")
                {
                    GameObject body = GameObject.Find(bodyName + "(Clone)");
                    body.GetComponent<Trajectory>().UpdateTrajectory();
                }

            }
            Debug.Log("POST exitoso: " + request.downloadHandler.text);
        }
    }

    void OpenFileDialog()
    {
        var path = "";
        var extensions = "txt";

        var paths = StandaloneFileBrowser.OpenFilePanel("Titulo", "", extensions, false);

        if (paths.Length > 0)
        {
            path = paths[0];

            SatelliteFactory.CreateSatellite(path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabPressCount++;

            // Alterna la visibilidad del panel TabMenu
            if (tabPressCount == 1)
            {
                panelTabMenu.SetActive(!panelTabMenu.activeSelf);
            }

            else if (tabPressCount == 2)
            {
                panelTabMenu.SetActive(!panelTabMenu.activeSelf);
                panelTrajectoryMenu.SetActive(!panelTrajectoryMenu.activeSelf);
            }

            else
            {
                tabPressCount = 0;
                panelTrajectoryMenu.SetActive(!panelTrajectoryMenu.activeSelf);
            }



            /* GameObject selectedPlanet_2 = GameObject.Find("Mercury(Clone)");
            Planet selectedPlanetInfo = selectedPlanet_2.GetComponent<Planet>();

            GameObject aphelion_object = GameObject.Find("aphelion");
            TextMeshProUGUI aphelion = aphelion_object.GetComponent<TextMeshProUGUI>();

            GameObject toggle_aphelion_object = GameObject.Find("toggle_aphelion");
            Toggle toggle_aphelion = toggle_aphelion_object.GetComponent<Toggle>();

            if (toggle_aphelion.isOn == false)
            {
                aphelion_object.SetActive(false);
            }
            else
            {
                aphelion_object.SetActive(true);
            } */
        }
    }
}
