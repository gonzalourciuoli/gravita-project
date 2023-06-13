using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class Planet : MonoBehaviour
{
    [SerializeField]
    public string alternative_name;
    [SerializeField]
    public float aphelion;
    [SerializeField]
    public float arg_periapsis;
    [SerializeField]
    public int avg_temp;
    [SerializeField]
    public float axial_tilt;
    [SerializeField]
    public string body_id;
    [SerializeField]
    public string body_name;
    [SerializeField]
    public string body_type;
    [SerializeField]
    public float density;
    [SerializeField]
    public string dimension;
    [SerializeField]
    public string discovered_by;
    [SerializeField]
    public string discovery_date;
    [SerializeField]
    public float eccentricity;
    [SerializeField]
    public float equa_radius;
    [SerializeField]
    public int escape;
    [SerializeField]
    public float flattening;
    [SerializeField]
    public float gravity;
    [SerializeField]
    public float inclination;
    [SerializeField]
    public bool is_planet;
    [SerializeField]
    public float long_asc_node;
    [SerializeField]
    public float main_anomaly;
    [SerializeField]
    public Mass mass;
    [SerializeField]
    public float mean_radius;
    [SerializeField]
    public List<Moon> moons;
    [SerializeField]
    public float perihelion;
    [SerializeField]
    public float polar_radius;
    [SerializeField]
    public float semimajor_axis;
    [SerializeField]
    public float sideral_orbit;
    [SerializeField]
    public float sideral_rotation;
    [SerializeField]
    public bool systeme_solaire_availability;
    [SerializeField]
    public Vol vol;

    public void Initialize(PlanetInfo planetInfo)
    {
        alternative_name = planetInfo.alternative_name;
        aphelion = planetInfo.aphelion;
        arg_periapsis = planetInfo.arg_periapsis;
        avg_temp = planetInfo.avg_temp;
        axial_tilt = planetInfo.axial_tilt;
        body_id = planetInfo.body_id;
        body_name = planetInfo.body_name;
        body_type = planetInfo.body_type;
        density = planetInfo.density;
        dimension = planetInfo.dimension;
        discovered_by = planetInfo.discovered_by;
        discovery_date = planetInfo.discovery_date;
        eccentricity = planetInfo.eccentricity;
        equa_radius = planetInfo.equa_radius;
        escape = planetInfo.escape;
        flattening = planetInfo.flattening;
        gravity = planetInfo.gravity;
        inclination = planetInfo.inclination;
        is_planet = planetInfo.is_planet;
        long_asc_node = planetInfo.long_asc_node;
        main_anomaly = planetInfo.main_anomaly;
        mass = planetInfo.mass;
        mean_radius = planetInfo.mean_radius;
        moons = planetInfo.moons;
        perihelion = planetInfo.perihelion;
        polar_radius = planetInfo.polar_radius;
        semimajor_axis = planetInfo.semimajor_axis;
        sideral_orbit = planetInfo.sideral_orbit;
        sideral_rotation = planetInfo.sideral_rotation;
        systeme_solaire_availability = planetInfo.systeme_solaire_availability;
        vol = planetInfo.vol;

        // Realiza cualquier otra inicialización necesaria
    }
}