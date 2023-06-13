using UnityEngine;
using TMPro;

public class InfoPanelInitializer
{
    public static TextMeshProUGUI alternative_name;
    public static TextMeshProUGUI aphelion;
    public static TextMeshProUGUI arg_periapsis;
    public static TextMeshProUGUI avg_temp;
    public static TextMeshProUGUI axial_tilt;
    public static TextMeshProUGUI body_id;
    public static TextMeshProUGUI body_name;
    public static TextMeshProUGUI body_type;
    public static TextMeshProUGUI density;
    public static TextMeshProUGUI dimension;
    public static TextMeshProUGUI discovered_by;
    public static TextMeshProUGUI discovery_date;
    public static TextMeshProUGUI eccentricity;
    public static TextMeshProUGUI equa_radius;
    public static TextMeshProUGUI escape;
    public static TextMeshProUGUI flattening;
    public static TextMeshProUGUI gravity;
    public static TextMeshProUGUI inclination;
    public static TextMeshProUGUI is_planet;
    public static TextMeshProUGUI long_asc_node;
    public static TextMeshProUGUI main_anomaly;
    public static TextMeshProUGUI mass;
    public static TextMeshProUGUI mean_radius;
    public static TextMeshProUGUI moons;
    public static TextMeshProUGUI perihelion;
    public static TextMeshProUGUI semimajor_axis;
    public static TextMeshProUGUI sideral_orbit;
    public static TextMeshProUGUI sideral_rotation;
    public static TextMeshProUGUI vol;


    public static void InitializeInfoPanel(PlanetInfo selectedPlanetInfo)
    {
        // Editamos la información del cuerpo
        GameObject alternative_name_object = GameObject.Find("alternative_name");
        alternative_name = alternative_name_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.alternative_name != "")
        {
            alternative_name.text = "Alternative name: " + selectedPlanetInfo.alternative_name;
        }
        else
        {
            alternative_name.enabled = false;
        }

        GameObject aphelion_object = GameObject.Find("aphelion");
        aphelion = aphelion_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.aphelion.ToString() != "")
        {
            aphelion.text = "Aphelion: " + selectedPlanetInfo.aphelion.ToString();
        }
        else
        {
            aphelion.enabled = false;
        }

        GameObject arg_periapsis_object = GameObject.Find("arg_periapsis");
        arg_periapsis = arg_periapsis_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.arg_periapsis.ToString() != "")
        {
            arg_periapsis.text = "Argument of periapsis: " + selectedPlanetInfo.arg_periapsis.ToString();
        }
        else
        {
            arg_periapsis.enabled = false;
        }

        GameObject avg_temp_object = GameObject.Find("avg_temp");
        avg_temp = avg_temp_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.avg_temp.ToString() != "")
        {
            avg_temp.text = "Average temperature: " + selectedPlanetInfo.avg_temp.ToString();
        }
        else
        {
            avg_temp.enabled = false;
        }

        GameObject axial_tilt_object = GameObject.Find("axial_tilt");
        axial_tilt = axial_tilt_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.axial_tilt.ToString() != "")
        {
            axial_tilt.text = "Axial tilt: " + selectedPlanetInfo.axial_tilt.ToString();
        }
        else
        {
            axial_tilt.enabled = false;
        }

        GameObject body_id_object = GameObject.Find("body_id");
        body_id = body_id_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.body_id != "")
        {
            body_id.text = "Body id: " + selectedPlanetInfo.body_id;
        }
        else
        {
            body_id.enabled = false;
        }

        GameObject body_name_object = GameObject.Find("body_name");
        body_name = body_name_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.body_name != "")
        {
            body_name.text = "Body name: " + selectedPlanetInfo.body_name;
        }
        else
        {
            body_name.enabled = false;
        }

        GameObject body_type_object = GameObject.Find("body_type");
        body_type = body_type_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.body_type != "")
        {
            body_type.text = "Body type: " + selectedPlanetInfo.body_type;
        }
        else
        {
            body_type.enabled = false;
        }

        GameObject density_object = GameObject.Find("density");
        density = density_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.density.ToString() != "")
        {
            density.text = "Density: " + selectedPlanetInfo.density.ToString();
        }
        else
        {
            density.enabled = false;
        }

        GameObject dimension_object = GameObject.Find("dimension");
        dimension = dimension_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.dimension != "")
        {
            dimension.text = "Dimension: " + selectedPlanetInfo.dimension;
        }
        else
        {
            dimension.enabled = false;
        }

        GameObject discovered_by_object = GameObject.Find("discovered_by");
        discovered_by = discovered_by_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.discovered_by != "")
        {
            discovered_by.text = "Discovered by: " + selectedPlanetInfo.discovered_by;
        }
        else
        {
            discovered_by.enabled = false;
        }

        GameObject discovery_date_object = GameObject.Find("discovery_date");
        discovery_date = discovery_date_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.discovery_date != "")
        {
            discovery_date.text = "Discovery date: " + selectedPlanetInfo.discovery_date;
        }
        else
        {
            discovery_date.enabled = false;
        }

        GameObject eccentricity_object = GameObject.Find("eccentricity");
        eccentricity = eccentricity_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.eccentricity.ToString() != "")
        {
            eccentricity.text = "Eccentricity: " + selectedPlanetInfo.eccentricity.ToString();
        }
        else
        {
            eccentricity.enabled = false;
        }

        GameObject equa_radius_object = GameObject.Find("equa_radius");
        equa_radius = equa_radius_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.equa_radius.ToString() != "")
        {
            equa_radius.text = "Equatorial radius: " + selectedPlanetInfo.equa_radius.ToString();
        }
        else
        {
            equa_radius.enabled = false;
        }

        GameObject escape_object = GameObject.Find("escape");
        escape = escape_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.escape.ToString() != "")
        {
            escape.text = "Escape: " + selectedPlanetInfo.escape.ToString();
        }
        else
        {
            escape.enabled = false;
        }

        GameObject flattening_object = GameObject.Find("flattening");
        flattening = flattening_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.flattening.ToString() != "")
        {
            flattening.text = "Flattening: " + selectedPlanetInfo.flattening.ToString();
        }
        else
        {
            flattening.enabled = false;
        }

        GameObject gravity_object = GameObject.Find("gravity");
        gravity = gravity_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.gravity.ToString() != "")
        {
            gravity.text = "Gravity: " + selectedPlanetInfo.gravity.ToString();
        }
        else
        {
            gravity.enabled = false;
        }

        GameObject inclination_object = GameObject.Find("inclination");
        inclination = inclination_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.inclination.ToString() != "")
        {
            inclination.text = "Inclination: " + selectedPlanetInfo.inclination.ToString();
        }
        else
        {
            inclination.enabled = false;
        }

        GameObject is_planet_object = GameObject.Find("is_planet");
        is_planet = is_planet_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.is_planet.ToString() != "")
        {
            is_planet.text = "Is planet: " + selectedPlanetInfo.is_planet.ToString();
        }
        else
        {
            is_planet.enabled = false;
        }

        GameObject long_asc_node_object = GameObject.Find("long_asc_node");
        long_asc_node = long_asc_node_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.long_asc_node.ToString() != "")
        {
            long_asc_node.text = "Longitude of ascending node: " + selectedPlanetInfo.long_asc_node.ToString();
        }
        else
        {
            long_asc_node.enabled = false;
        }

        GameObject main_anomaly_object = GameObject.Find("main_anomaly");
        main_anomaly = main_anomaly_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.main_anomaly.ToString() != "")
        {
            main_anomaly.text = "Main anomaly: " + selectedPlanetInfo.main_anomaly.ToString();
        }
        else
        {
            main_anomaly.enabled = false;
        }

        GameObject mass_object = GameObject.Find("mass");
        mass = mass_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.mass.ToString() != "")
        {
            mass.text = "Mass: " + selectedPlanetInfo.mass.ToString();
        }
        else
        {
            mass.enabled = false;
        }

        GameObject mean_radius_object = GameObject.Find("mean_radius");
        mean_radius = mean_radius_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.mean_radius.ToString() != "")
        {
            mean_radius.text = "Mean radius: " + selectedPlanetInfo.mean_radius.ToString();
        }
        else
        {
            mean_radius.enabled = false;
        }

        GameObject moons_object = GameObject.Find("moons");
        moons = moons_object.GetComponent<TextMeshProUGUI>();

        /* moons.text = selectedPlanetInfo.moons.ToString(); */

        GameObject perihelion_object = GameObject.Find("perihelion");
        perihelion = perihelion_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.perihelion.ToString() != "")
        {
            perihelion.text = "Perihelion: " + selectedPlanetInfo.perihelion.ToString();
        }
        else
        {
            perihelion.enabled = false;
        }

        GameObject semimajor_axis_object = GameObject.Find("semimajor_axis");
        semimajor_axis = semimajor_axis_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.semimajor_axis.ToString() != "")
        {
            semimajor_axis.text = "Semimajor axis: " + selectedPlanetInfo.semimajor_axis.ToString();
        }
        else
        {
            semimajor_axis.enabled = false;
        }

        GameObject sideral_orbit_object = GameObject.Find("sideral_orbit");
        sideral_orbit = sideral_orbit_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.sideral_orbit.ToString() != "")
        {
            sideral_orbit.text = "Sideral orbit: " + selectedPlanetInfo.sideral_orbit.ToString();
        }
        else
        {
            sideral_orbit.enabled = false;
        }

        GameObject sideral_rotation_object = GameObject.Find("sideral_rotation");
        sideral_rotation = sideral_rotation_object.GetComponent<TextMeshProUGUI>();

        if (selectedPlanetInfo.sideral_rotation.ToString() != "")
        {
            sideral_rotation.text = "Sideral rotation: " + selectedPlanetInfo.sideral_rotation.ToString();
        }
        else
        {
            sideral_rotation.enabled = false;
        }

        GameObject vol_object = GameObject.Find("vol");
        vol = vol_object.GetComponent<TextMeshProUGUI>();

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
