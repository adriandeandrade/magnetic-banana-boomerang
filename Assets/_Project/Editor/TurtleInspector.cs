using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LittleTurtle))]
public class TurtleInspector : Editor
{
	private void OnSceneGUI()
	{
        base.OnInspectorGUI();
		LittleTurtle turtle = (LittleTurtle)target;

        // Draw flee threshold
        float fleeThresold = turtle.FleeThreshold;
        Handles.color = Color.blue;
        Handles.DrawWireDisc(turtle.transform.position, new Vector3(0f, 0f, 1f), fleeThresold);

        // Draw flee threshold
        float fleeDestinationSearchRadius = turtle.PickFleeDestinationRadius;
        Handles.color = Color.green;
        Handles.DrawWireDisc(turtle.transform.position, new Vector3(0f, 0f, 1f), fleeDestinationSearchRadius);

        // Draw flee threshold
        float knockbackRadius = turtle.DefensiveKnockbackRadius;
        Handles.color = Color.white;
        Handles.DrawWireDisc(turtle.transform.position, new Vector3(0f, 0f, 1f), knockbackRadius);
	}
}
