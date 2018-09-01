using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor {

    private Planet planet;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldOut);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldOut);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut)
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
           foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);

            if (foldOut)
            {
                Editor editor = CreateEditor(settings);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
