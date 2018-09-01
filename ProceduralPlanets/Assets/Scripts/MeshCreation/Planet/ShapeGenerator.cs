﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator {

    private ShapeSettings settings;
    private NoiseFilter[] noiseFilters;

    public ShapeGenerator(ShapeSettings _settings)
    {
        this.settings = _settings;
        noiseFilters = new NoiseFilter[settings.noiseLayers.Length];

        for(int i = 0; i <noiseFilters.Length; ++i)
        {
            noiseFilters[i] = new NoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(_pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for(int i = 1; i < noiseFilters.Length; ++i)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(_pointOnUnitSphere) * mask;
            }
        }
        return _pointOnUnitSphere * settings.planetRadius * (1 + elevation);
    }
}