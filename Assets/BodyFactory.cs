using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFactory
{
    public ICelestialBody CreateCelestialBody(string bodyType)
    {
        switch (bodyType)
        {
            case "Planet":
                return new Planet();
            case "Satellite":
                return new Satellite();
            default:
                throw new ArgumentException("Invalid celestial body type");
        }
    }
}
