﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(CameraMovement))]
public class CameraMovementEditor : Editor {

	public override void OnInspectorGUI()
    {
        CameraMovement myScript = (CameraMovement) target;
        myScript.player = (GameObject   )EditorGUILayout.ObjectField("Target",myScript.player,typeof(GameObject),true);
        myScript.smoothTime= EditorGUILayout.FloatField("Smoothness", myScript.smoothTime);
        myScript.hasDeadZone = EditorGUILayout.Toggle("Dead zone", myScript.hasDeadZone);
        if(myScript.hasDeadZone)
        {
            myScript.marginX = EditorGUILayout.FloatField("Margin X", myScript.marginX);
            myScript.marginY = EditorGUILayout.FloatField("Margin Y", myScript.marginY);
        }

    }
}