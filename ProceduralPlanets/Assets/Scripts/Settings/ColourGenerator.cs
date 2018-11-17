﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator {

    private ColourSettings settings;
    private Texture2D texture;
    private const int textureResolution = 50;
    private INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColourSettings _settings)
    {
        settings = _settings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdatedElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + .001f;

        for (int i = 0; i < numBiomes; ++i)
        {
            // calculating the distance of the biomes start height
            // from the current height percent
            float dst = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1,numBiomes - 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;
        foreach (var biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; ++i)
            {
                Color gradientCol;
                // sample ocean gradient
                if (i < textureResolution)
                {
                    gradientCol = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
                }
                // sample from the biome gradient
                else
                {
                    gradientCol = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }
               
                Color tintColor = biome.tint;
                colors[colorIndex] = gradientCol * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
