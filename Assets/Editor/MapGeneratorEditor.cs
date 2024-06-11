using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime;
using System.Diagnostics;

[CustomEditor(typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
  public override void OnInspectorGUI()
    {
        MapGenerator mapgen = (MapGenerator)target;


        if(DrawDefaultInspector())
        {
            if(mapgen.AutoUpdate) 
            {
                mapgen.GenerateMap();
            }
        }
        
        if(GUILayout.Button("Generate"))
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            mapgen.GenerateMap();

            stopwatch.Stop();
            UnityEngine.Debug.Log("GenerateMap took " + stopwatch.ElapsedMilliseconds + " ms");
        }
    }
}
