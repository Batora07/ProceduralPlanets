using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter{

    private NoiseSettings.RigidNoiseSettings settings;
    private Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings _settings)
    {
        this.settings = _settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; ++i)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v;
            // regions starting really low down will remain relatively undetailed
            // compared to regions that start higher up.
            // we're increasing the layers of noise depending of the weight
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weigthMultiplier);

            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}
