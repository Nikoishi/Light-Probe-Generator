#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class CustomEditorButton : EditorWindow
{
    // Variables to input a LightProbeGroup and a Vector3
    private LightProbeGroup lightProbeGroup;
    private Vector3 probeStartPosition;
    private Vector3 probeSize;
    private float density;
    private float maxDistanceFromGeometry;

    // Add a menu item to open the window
    [MenuItem("Tools/Generate Light Probes")]
    public static void ShowWindow()
    {
        // Opens the window and gives it a title
        GetWindow<CustomEditorButton>("Generate Light Probes");
    }

    private void OnGUI()
    {
        // Input fields for the LightProbeGroup and the Vector3
        lightProbeGroup = (LightProbeGroup)EditorGUILayout.ObjectField("Light Probe Group", lightProbeGroup, typeof(LightProbeGroup), true);
        probeStartPosition = EditorGUILayout.Vector3Field("Start Pos", probeStartPosition);
        probeSize = EditorGUILayout.Vector3Field("Volume", probeSize);
        density = EditorGUILayout.FloatField("probe Density", density);
        maxDistanceFromGeometry = EditorGUILayout.FloatField("maxDistanceFromGeometry", maxDistanceFromGeometry);

        // Create a button in the editor window
        if (GUILayout.Button("Generate Probes"))
        {
            // Execute the action when the button is pressed
            if (lightProbeGroup == null)
            {
                Debug.LogError("Please assign a Light Probe Group.");
            }
            else
            {
                lightProbeGroup = PerformAction(lightProbeGroup, probeSize, probeStartPosition, density);
            }
        }
    }

    // The method that is triggered when the button is pressed
    private LightProbeGroup PerformAction(in LightProbeGroup lPG, in Vector3 volume, in Vector3 startPos, in float density)
    {
        if (lightProbeGroup != null)
        {
            float xMax = volume.x;
            float yMax = volume.y;
            float zMax = volume.z;
            float xInc = volume.x / Mathf.Clamp(density, 0.1f, 100f);
            float yInc = volume.y / Mathf.Clamp(density, 0.1f, 100f);
            float zInc = volume.z / Mathf.Clamp(density, 0.1f, 100f);
            List<Vector3> pos = new List<Vector3>();

            for (float x = 0; x < xMax; x += xInc)
            {
                for (float y = 0; y < yMax; y += yInc)
                {
                    for (float z = 0; z < zMax; z += zInc)
                    {
                        Vector3 curPos = new Vector3(x + startPos.x, y + startPos.y, z + startPos.z);
                        if (Physics.CheckSphere(curPos,maxDistanceFromGeometry) && !Physics.CheckSphere(curPos, 0.1f))
                        {
                            pos.Add(curPos);
                        }
                    }
                }
            }

            lPG.probePositions = pos.ToArray();
            return lPG;
        }
        else
        {
            return lPG;
        }
    }
}

#endif
