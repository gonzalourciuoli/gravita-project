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
    BodyFactory bodyFactory = new BodyFactory();
    public Button studentButton;
    public Button educatorButton;
    string lastBody;
    public GameObject infoPanelContent;
    public GameObject infoToggleContent;
    public GameObject planetToggleMenu;
    List<string> previousEnabledList = new List<string>();
    List<string> enabledList = new List<string>();
    public GameObject panelSliderMenu;
    private GameObject selectedPlanet;
    public GameObject panelInfo;
    public GameObject planetNameTextObject;
    public GameObject panelTabMenu;
    public GameObject panelTrajectoryMenu;
    public Button calculateTrajectoriesButton;
    public Slider progressBar;
    public Button loadTrajectoriesButton;
    public Toggle toggleAlternativeName;
    public Toggle toggleAphelion;
    public Toggle toggleArgPeriapsis;
    public Toggle toggleAvgTemp;
    public Toggle toggleAxialTilt;
    public Toggle toggleBodyId;
    public Toggle toggleBodyName;
    public Toggle toggleBodyType;
    public Toggle toggleDensity;
    public Toggle toggleDimension;
    public Toggle toggleDiscoveredBy;
    public Toggle toggleDiscoveryDate;
    public Toggle toggleEccentricity;
    public Toggle toggleEquaRadius;
    public Toggle toggleEscape;
    public Toggle toggleFlattening;
    public Toggle toggleGravity;
    public Toggle toggleInclination;
    public Toggle toggleIsPlanet;
    public Toggle toggleLongAscNode;
    public Toggle toggleMainAnomaly;
    public Toggle toggleMass;
    public Toggle toggleMeanRadius;
    public Toggle toggleMoons;
    public Toggle togglePerihelion;
    public Toggle toggleSemimajorAxis;
    public Toggle toggleSideralOrbit;
    public Toggle toggleSideralRotation;
    public Toggle toggleVol;


    private int tabPressCount = 0;
    bool[] infoShow = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    string[] info = { "alternative_name", "aphelion", "arg_periapsis", "avg_temp", "axial_tilt", "body_id", "body_name", "body_type", "density", "dimension", "discovered_by", "discovery_date",
    "eccentricity", "equa_radius", "escape", "flattening", "gravity", "inclination", "is_planet", "long_asc_node", "main_anomaly", "mass", "mean_radius", "moons", "perihelion", "semimajor_axis",
    "sideral_orbit", "sideral_rotation", "vol" };
    private List<GameObject> bodyList = new List<GameObject>();
    private string rol = "student";

    // Start is called before the first frame update
    void Start()
    {
        AddPlanetNamesToMenu();

        Toggle[] toggles = planetToggleMenu.GetComponentsInChildren<Toggle>();

        studentButton.onClick.AddListener(OnStudentButtonClick);
        educatorButton.onClick.AddListener(OnEducatorButtonClick);

        // Esto es para gestionar los toggles de los planetas
        /* foreach (string bodyName in bodyNames)
        {
            GameObject body = GameObject.Find(bodyName + "(Clone)");
            bodyList.Add(body);
        }

        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle, bodyList); });
        } */

        panelInfo.SetActive(false);
        panelTabMenu.SetActive(false);

        // Encuentra el botón ShowPlanisphereButton y agrega el método OnShowPlanisphereButtonClick como listener
        Button showPlanisphereButton = GameObject.Find("ShowPlanisphereButton").GetComponent<Button>();
        showPlanisphereButton.onClick.AddListener(OnShowPlanisphereButtonClick);

        calculateTrajectoriesButton.onClick.AddListener(OnCalculateTrajectoriesButtonClick);

        loadTrajectoriesButton.onClick.AddListener(OpenFileDialog);
    }

    private void OnStudentButtonClick()
    {
        rol = "student";

        Image imageStudentComponent = studentButton.GetComponent<Image>();
        Image imageEducatorComponent = educatorButton.GetComponent<Image>();

        imageStudentComponent.color = new Color(0.635f, 0.635f, 0.635f);
        imageEducatorComponent.color = new Color(1f, 1f, 1f);

        tabPressCount = 0;

        planetToggleMenu.SetActive(false);
        panelTabMenu.SetActive(false);
        panelTrajectoryMenu.SetActive(false);
    }

    private void OnEducatorButtonClick()
    {
        rol = "educator";

        Image imageStudentComponent = studentButton.GetComponent<Image>();
        Image imageEducatorComponent = educatorButton.GetComponent<Image>();

        imageEducatorComponent.color = new Color(0.635f, 0.635f, 0.635f);
        imageStudentComponent.color = new Color(1f, 1f, 1f);
        
        tabPressCount = 0;

        planetToggleMenu.SetActive(false);
        panelTabMenu.SetActive(false);
        panelTrajectoryMenu.SetActive(false);
    }

    private void OnToggleValueChanged(Toggle toggle, List<GameObject> bodyList)
    {
        if (toggle.isOn)
        {
            GameObject toTogglePlanet = GameObject.Find(toggle.name.Replace("toggle_", "") + "(Clone)");
            toTogglePlanet.SetActive(true);
        }
        else
        {
            GameObject toTogglePlanet = GameObject.Find(toggle.name.Replace("toggle_", "") + "(Clone)");
            toTogglePlanet.SetActive(false);
        }
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
            backgroundImage.color = new Color(0.984f, 0.949f, 0.937f, 1f); // Color gris-negro

            // Crea un nuevo objeto Text como hijo del contenedor
            GameObject newText = new GameObject(bodyName + "_Text");
            newText.AddComponent<Text>();

            // Configura el componente Text
            Text textComponent = newText.GetComponent<Text>();
            textComponent.text = bodyName;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = new Color(0.211f, 0.427f, 0.741f, 1.0f);
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

        GameObject nameContainer = GameObject.Find(bodyName + "_Container");
        Image imageComponent = nameContainer.GetComponent<Image>();

        // Si se hace clic en el mismo nombre de planeta, CameraSetting.Instance.target será null
        if (selectedPlanet == newSelectedPlanet)
        {
            imageComponent.color = new Color(0.984f, 0.949f, 0.937f);

            CameraSetting.Instance.target = null;
            selectedPlanet = null;

            // Si seleccionamos el mismo planeta que ya estabamos seleccionando, desactivamos el panel
            panelInfo.SetActive(false);
        }
        else
        {
            if (selectedPlanet != null)
            {
                GameObject selectedPlanetNameContainer = GameObject.Find(selectedPlanet.name.Replace("(Clone)", "_Container"));
                Image selectedPlanetImageComponent = selectedPlanetNameContainer.GetComponent<Image>();
                selectedPlanetImageComponent.color = new Color(0.984f, 0.949f, 0.937f);
            }

            imageComponent.color = new Color(0.635f, 0.635f, 0.635f);

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

            PlanetInfo selectedPlanetInfo = newSelectedPlanet.GetComponent<PlanetInfo>();

            InfoPanelInitializer.InitializeInfoPanel(selectedPlanetInfo);
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
                    body.GetComponent<PlanetTrajectory>().UpdateTrajectory();
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

            ICelestialBody satelliteObject = bodyFactory.CreateCelestialBody("Satellite");

            satelliteObject.Initialize("Satellite", path);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (rol == "student")
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                tabPressCount++;

                if (tabPressCount == 1)
                {
                    panelTrajectoryMenu.SetActive(true);
                }
                else
                {
                    panelTrajectoryMenu.SetActive(false);
                    tabPressCount = 0;
                }
            }
        }

        else if (rol == "educator")
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                tabPressCount++;

                if (tabPressCount == 1)
                {
                    panelTrajectoryMenu.SetActive(true);
                }
                else if (tabPressCount == 2)
                {
                    panelTrajectoryMenu.SetActive(false);
                    planetToggleMenu.SetActive(true);
                }
                else if (tabPressCount == 3)
                {
                    planetToggleMenu.SetActive(false);
                    panelTabMenu.SetActive(true);
                } else {
                    panelTabMenu.SetActive(false);
                    tabPressCount = 0;
                }
            }
        }

        if (InfoPanelInitializer.alternative_name != null)
        {
            if (toggleAlternativeName.isOn)
            {
                if (!InfoPanelInitializer.alternative_name.gameObject.activeSelf)
                {
                    InfoPanelInitializer.alternative_name.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.alternative_name.gameObject.activeSelf)
                {
                    InfoPanelInitializer.alternative_name.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.aphelion != null)
        {
            if (toggleAphelion.isOn)
            {
                if (!InfoPanelInitializer.aphelion.gameObject.activeSelf)
                {
                    InfoPanelInitializer.aphelion.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.aphelion.gameObject.activeSelf)
                {
                    InfoPanelInitializer.aphelion.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.arg_periapsis != null)
        {
            if (toggleArgPeriapsis.isOn)
            {
                if (!InfoPanelInitializer.arg_periapsis.gameObject.activeSelf)
                {
                    InfoPanelInitializer.arg_periapsis.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.arg_periapsis.gameObject.activeSelf)
                {
                    InfoPanelInitializer.arg_periapsis.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.avg_temp != null)
        {
            if (toggleAvgTemp.isOn)
            {
                if (!InfoPanelInitializer.avg_temp.gameObject.activeSelf)
                {
                    InfoPanelInitializer.avg_temp.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.avg_temp.gameObject.activeSelf)
                {
                    InfoPanelInitializer.avg_temp.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.axial_tilt != null)
        {
            if (toggleAxialTilt.isOn)
            {
                if (!InfoPanelInitializer.axial_tilt.gameObject.activeSelf)
                {
                    InfoPanelInitializer.axial_tilt.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.axial_tilt.gameObject.activeSelf)
                {
                    InfoPanelInitializer.axial_tilt.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.body_id != null)
        {
            if (toggleBodyId.isOn)
            {
                if (!InfoPanelInitializer.body_id.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_id.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.body_id.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_id.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.body_name != null)
        {
            if (toggleBodyName.isOn)
            {
                if (!InfoPanelInitializer.body_name.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_name.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.body_name.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_name.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.body_type != null)
        {
            if (toggleBodyType.isOn)
            {
                if (!InfoPanelInitializer.body_type.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_type.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.body_type.gameObject.activeSelf)
                {
                    InfoPanelInitializer.body_type.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.density != null)
        {
            if (toggleDensity.isOn)
            {
                if (!InfoPanelInitializer.density.gameObject.activeSelf)
                {
                    InfoPanelInitializer.density.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.density.gameObject.activeSelf)
                {
                    InfoPanelInitializer.density.gameObject.SetActive(false);
                }
            }
        }

        if (InfoPanelInitializer.dimension != null)
        {
            if (toggleDimension.isOn)
            {
                if (!InfoPanelInitializer.dimension.gameObject.activeSelf)
                {
                    InfoPanelInitializer.dimension.gameObject.SetActive(true);
                }
            }
            else
            {
                if (InfoPanelInitializer.dimension.gameObject.activeSelf)
                {
                    InfoPanelInitializer.dimension.gameObject.SetActive(false);
                }
            }
        }
    }
}
