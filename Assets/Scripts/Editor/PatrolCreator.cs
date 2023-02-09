using Necropanda.AI;
using Necropanda.Utils.Debugger;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatrolCreator : EditorWindow
{
    private EnemyAI aiController = null;
    private List<GameObject> patrolPoints = new List<GameObject>();


    [MenuItem("Corruption of Arcana Chapter 2/Patrol Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<PatrolCreator>();
        window.titleContent = new GUIContent("Patrol Creator");
        window.Show();
    }

    private void OnGUI()
    {
        #region Tool Description
        GUILayout.Space(2.5f);
        GUILayout.Label("What is this tool?");
        GUILayout.Space(2.5f);
        GUILayout.Label("This tool is a simple tool that was made for ");
        GUILayout.Label("making AI patrols, easier than ever before!");
        GUILayout.Space(5);
        GUILayout.Label("How do I use this tool?");
        GUILayout.Space(2.5f);
        GUILayout.Label("1) Select an AI in the gameworld.");
        GUILayout.Label("2) Click \"place new point\".");
        GUILayout.Label("3) Move the newly created point to the desired location.");
        GUILayout.Label("4) Once all points have been placed, click save!");
        GUILayout.Label("5) Done!");
        GUILayout.Space(2.5f);
        #endregion

        GUILayout.Space(40);
        GUILayout.Label("Step 1: Select an AI (under the scene tab not the assets tab)");
        aiController = EditorGUILayout.ObjectField("AI: ", aiController, typeof(Object), true) as EnemyAI;
        GUILayout.Space(10);
        GUILayout.Label("Step 2: Place a new point.");
        if (GUILayout.Button("Spawn Patrol Point"))
        {
            SetupPatrolPoint();
        }
    }

    private void SetupPatrolPoint()
    {
        GameObject patrolPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        patrolPoint.name = "Patrol Point";
        MeshRenderer meshRenderer = patrolPoint.GetComponent<MeshRenderer>();
        // // add the needed components
        // MeshFilter mesh = patrolPoint.AddComponent<MeshFilter>();
        // // Create new primitive for getting the mesh
        // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // mesh.sharedMesh = sphere.GetComponent<MeshFilter>().sharedMesh;
        // // Destroy the primitive once mesh has been set
        // DestroyImmediate(sphere);

        // Set the patrol point color
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = Color.red;

        meshRenderer.material = mat;

        string tag = "Patrol Point";

        UnityEditorInternal.InternalEditorUtility.AddTag(tag);

        // Tag the gameobject for cleanup
        patrolPoint.tag = tag;

        // Add it to the list
        patrolPoints.Add(patrolPoint);
    }

    #region Points list functions
    /// <summary>
    /// Clears the Patrol Points list
    /// </summary>
    private void ClearList()
    {
        Debugger.instance.SendDebug($"Clearing list of {patrolPoints.Count} points.");
        patrolPoints = new List<GameObject>();
        Debugger.instance.SendDebug("Patrol Points Cleared.");
    }

    /// <summary>
    /// Remove last in the list of Patrol Points
    /// </summary>
    private void RemoveLast()
    {
        int lastInList = patrolPoints.Count;
        patrolPoints.RemoveAt(lastInList);
    }

    /// <summary>
    /// Saves the patrol point onto the AI for use in game scenarios
    /// </summary>
    private void SavePatrolPoints()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}