using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetJson
{
    public string alternative_name { get; set; }
    public float aphelion { get; set; }
    public float arg_periapsis { get; set; }
    public int avg_temp { get; set; }
    public float axial_tilt { get; set; }
    public string body_id { get; set; }
    public string body_name { get; set; }
    public string body_type { get; set; }
    public float density { get; set; }
    public string dimension { get; set; }
    public string discovered_by { get; set; }
    public string discovery_date { get; set; }
    public float eccentricity { get; set; }
    public float equa_radius { get; set; }
    public int escape { get; set; }
    public float flattening { get; set; }
    public float gravity { get; set; }
    public float inclination { get; set; }
    public bool is_planet { get; set; }
    public float long_asc_node { get; set; }
    public float main_anomaly { get; set; }
    public Mass mass { get; set; }
    public float mean_radius { get; set; }
    public List<Moon> moons { get; set; }
    public float perihelion { get; set; }
    public float polar_radius { get; set; }
    public float semimajor_axis { get; set; }
    public float sideral_orbit { get; set; }
    public float sideral_rotation { get; set; }
    public bool systeme_solaire_availability { get; set; }
    public Vol vol { get; set; }
}
