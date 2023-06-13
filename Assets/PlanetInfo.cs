using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class PlanetInfo : MonoBehaviour
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

    public void Initialize(PlanetJson planetJson)
    {
        alternative_name = planetJson.alternative_name;
        aphelion = planetJson.aphelion;
        arg_periapsis = planetJson.arg_periapsis;
        avg_temp = planetJson.avg_temp;
        axial_tilt = planetJson.axial_tilt;
        body_id = planetJson.body_id;
        body_name = planetJson.body_name;
        body_type = planetJson.body_type;
        density = planetJson.density;
        dimension = planetJson.dimension;
        discovered_by = planetJson.discovered_by;
        discovery_date = planetJson.discovery_date;
        eccentricity = planetJson.eccentricity;
        equa_radius = planetJson.equa_radius;
        escape = planetJson.escape;
        flattening = planetJson.flattening;
        gravity = planetJson.gravity;
        inclination = planetJson.inclination;
        is_planet = planetJson.is_planet;
        long_asc_node = planetJson.long_asc_node;
        main_anomaly = planetJson.main_anomaly;
        mass = planetJson.mass;
        mean_radius = planetJson.mean_radius;
        moons = planetJson.moons;
        perihelion = planetJson.perihelion;
        polar_radius = planetJson.polar_radius;
        semimajor_axis = planetJson.semimajor_axis;
        sideral_orbit = planetJson.sideral_orbit;
        sideral_rotation = planetJson.sideral_rotation;
        systeme_solaire_availability = planetJson.systeme_solaire_availability;
        vol = planetJson.vol;
    }
}