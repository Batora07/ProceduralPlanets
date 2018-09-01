using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {

    private ShapeSettings settings;

    public ShapeGenerator(ShapeSettings _settings)
    {
        this.settings = _settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere)
    {
        return _pointOnUnitSphere * settings.planetRadius;
    }
}
