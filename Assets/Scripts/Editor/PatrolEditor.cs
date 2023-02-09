using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Authored & Written by Hayley Davies https://linktr.ee/cdgamedev
/// Supported by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI.Movement
{
    [CustomEditor(typeof(Patrol))]
    public class PatrolEditor : Editor
    {
        /// <summary>
        /// Mode stores the current state of the PatrolEditor
        /// </summary>
        enum Mode
        {
            None,
            Edit,
            Preview
        }

        // set the intial state to none
        Mode mode = Mode.None;
        // set the inital point to zero
        int point = 0;

        // delare a reference to the patrol
        Patrol patrol;
        // declare a List which stores the patrol points
        List<Vector3> patrolPoints;

        /// <summary>
        /// When the object is deselected in the scene view of the Unity Editor
        /// </summary>
        private void OnDisable()
        {
            StopPreview();
        }

        /// <summary>
        /// Every frame the SceneGUI updates
        /// </summary>
        private void OnSceneGUI()
        {
            // Get target and cast to Patrol
            patrol = (Patrol)target;

            // Set the patrolPoints to be the patrolPoints contained in the Patrol class
            patrolPoints = patrol.patrolPoints;

            // Draw the lines between patrol points
            DrawLines();

            // Draw the handles for the patrol points
            DrawPointHandles();
        }

        /// <summary>
        /// Show a rough preview of the patrol path (exluding NavMeshAgent pathing)
        /// </summary>
        private void PreviewPatrol()
        {

            // Get the transform of the patrol
            Transform transform = patrol.transform;

            // If the transform is currently at the selected patrol point
            if (transform.position == patrol.patrolPoints[point])
            {
                // Increment the point to the next point in the patrol points List
                // Ensure that the value is within the bounds of the List
                point = (point + 1) % patrol.patrolPoints.Count;
            }

            // Progresses the transform towards the patrol point location
            transform.position = Vector3.MoveTowards(transform.position, patrol.patrolPoints[point], 3 * Time.deltaTime);
        }


        /// <summary>
        /// Every frame the InspectorGUI updates
        /// </summary>
        public override void OnInspectorGUI()
        {
            // draw the default GUI
            base.OnInspectorGUI();

            // draw the buttons
            DrawButtons();
        }

        /// <summary>
        /// Draw the buttons which are shown on the Patrol component in the inspector
        /// </summary>
        private void DrawButtons()
        {
            // check the value of mode
            switch (mode)
            {
                // if there is no mode, draw all the buttons
                case Mode.None:
                    StartEditButton();
                    StartPreviewButton();
                    break;
                // if in edit mode, draw the end edit button
                case Mode.Edit:
                    EndEditButton();
                    break;
                // if in the preview mode, draw the end preview button
                case Mode.Preview:
                    EndPreviewButton();
                    break;
            }
        }

        /// <summary>
        /// Draw the button which starts the editing mode
        /// </summary>
        private void StartEditButton()
        {
            // draw a button with a label
            if (GUILayout.Button("Edit Patrol Path"))
            {
                // set the mode to edit
                mode = Mode.Edit;
            }
        }

        /// <summary>
        /// Draw the button which ends the editing mode
        /// </summary>
        private void EndEditButton()
        {
            // draw a button with a label
            if (GUILayout.Button("End Edit"))
            {
                // set the mode to none
                mode = Mode.None;
            }
        }

        /// <summary>
        /// Draw the button which starts the preview mode
        /// </summary>
        private void StartPreviewButton()
        {
            // draw a button with a label
            if (GUILayout.Button("Preview Patrol"))
            {
                // stop the preview
                StopPreview();
                // set the mode to preview
                mode = Mode.Preview;
                // add the PreviewPatrol function to the EditorApplication update event system
                EditorApplication.update += PreviewPatrol;
            }
        }

        /// <summary>
        /// Draw the button which ends the preview mode
        /// </summary>
        private void EndPreviewButton()
        {
            // draw a button with a label
            if (GUILayout.Button("Stop Preview"))
            {
                // stop the preview
                StopPreview();
            }
        }

        /// <summary>
        /// Stop preview mode from running
        /// </summary>
        private void StopPreview()
        {
            // if the patrol is null, return
            if (patrol == null)
            {
                return;
            }
            // reset the point value to start at the initial point
            point = 0;
            // set the mode to none
            mode = Mode.None;
            // set the transform to be located at the initial point
            patrol.transform.position = patrol.patrolPoints[0];
            // clear the event system for EditorApplication update
            EditorApplication.update = null;
        }

        /// <summary>
        /// Draw the handles on the patrol points
        /// </summary>
        private void DrawPointHandles()
        {
            // if the amount of points is zero
            if (patrolPoints.Count == 0)
            {
                // add the position of the transform to the patrolPoints list
                patrolPoints.Add(patrol.transform.position);
            }

            // draw the default handles
            DrawDefaultHandles(patrolPoints);

            // if in edit mode
            if (mode == Mode.Edit)
            {
                // set the initial patrol point to the position of the transform
                patrolPoints[0] = patrol.transform.position;

                // draw the move handles
                DrawMoveHandles(patrolPoints);
            }
        }

        /// <summary>
        /// Draw the move handles which will set the position of the points
        /// </summary>
        /// <param name="points">a list of patrol points</param>
        private void DrawMoveHandles(List<Vector3> points)
        {
            // iterate through the points
            for (int i = 1; i < points.Count; i++)
            {
                // draw a positional handle at the patrol point position
                Vector3 handlePosition = Handles.DoPositionHandle(points[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(patrol, "Change Look At Target Position");
                    // set the patrol point to the position of the handle
                    points[i] = handlePosition;
                }
            }
        }

        /// <summary>
        /// Draw the default handles at the positions of the patrol points
        /// </summary>
        /// <param name="points">a list of patrol points</param>
        private void DrawDefaultHandles(List<Vector3> points)
        {
            // set the handle colour to red
            Handles.color = Color.red;
            // draw a sphere at the final point
            Handles.SphereHandleCap(0, points[points.Count - 1], Quaternion.identity, 1f, EventType.Repaint);

            // set the handle colour to green
            Handles.color = Color.green;
            // iterate through the points
            for (int i = 0; i < points.Count - 1; i++)
            {
                // set the direction
                Vector3 direction = (points[i + 1] - points[i]).normalized;
                // set the rotation for the handle cap to face in
                Quaternion rotation = Quaternion.LookRotation(direction);

                // offset the cone object to ensure it starts at its base and not at its center
                Vector3 offset = (rotation * Vector3.forward) / 2;

                // draw the default handle cap
                Handles.ConeHandleCap(0, points[i] + offset, rotation, 1f, EventType.Repaint);
            }
        }

        /// <summary>
        /// Draw lines between the patrol points
        /// </summary>
        private void DrawLines()
        {
            // set the handle colour to white
            Handles.color = Color.white;

            // draw a line through all the points
            Handles.DrawPolyLine(patrolPoints.ToArray());
        }
    }
}