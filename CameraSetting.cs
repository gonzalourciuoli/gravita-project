using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CameraSetting : MonoBehaviour
{
    public Transform target;
    public float distance = 30.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zoomSpeed = 2.0f;
    public float minDistance = 0.1f;
    public float maxDistance = 100.0f;

    public Camera cam;
    public GameObject panelInfo;
    public TextMeshProUGUI planetNameText;
    public Slider timeSlider;
    private bool clickStartedOnUI = false;
    private float x = 0.0f;
    private float y = 0.0f;
    private float targetX;
    private float targetY;
    private float smoothXVelocity;
    private float smoothYVelocity;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private static CameraSetting _instance;
    public static CameraSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CameraSetting();
            }
            return _instance;
        }
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        cam = GetComponent<Camera>();
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 10000000.0f;

        /* cam.transform.rotation = Quaternion.Euler(-80, 0, 0);
        cam.transform.position = new Vector3(0, -98, -10); */

        panelInfo = GameObject.Find("InfoPanel");

        timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();

        timeSlider.onValueChanged.AddListener(OnTimeSliderValueChanged);
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == target)
                {
                    target = null;
                }
                else
                {
                    target = hit.transform;
                    float scaleFactor = target.localScale.x;
                    distance = 2.5f * scaleFactor;
                    minDistance = distance;
                    maxDistance = 30.0f * scaleFactor;
                }
            }
        }

        // Si se hace clic con el botón izquierdo del mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica si el puntero del mouse está sobre un objeto de la interfaz
            clickStartedOnUI = EventSystem.current.IsPointerOverGameObject();
        }

        if (target && Input.GetMouseButton(0) && !clickStartedOnUI)
        {
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        // Restablece el valor de clickStartedOnUI cuando se suelta el botón izquierdo del mouse
        if (Input.GetMouseButtonUp(0))
        {
            clickStartedOnUI = false;
        }

        if (target)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
            }

            float smoothTime = 0.1f;
            x = Mathf.SmoothDampAngle(x, targetX, ref smoothXVelocity, smoothTime);
            y = Mathf.SmoothDampAngle(y, targetY, ref smoothYVelocity, smoothTime);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public void OnTimeSliderValueChanged(float value)
    {
        Time.timeScale = value * 10;
    }
}
