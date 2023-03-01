using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* TODO
-Implement save -- DONE
-Implement delete -- DONE
-Implement randomize range 
-Get surface detection working -- DONE (mostly)
*/

public class MattsMassAssetPlacer : EditorWindow
{
    public GameObject asset = null;
    public int count = 0;
    public bool placeOnSurface = false;
    
    private List<GameObject> clones = new List<GameObject>();
    public LayerMask snappingSurfaces = ~0;
    private float rcRange = Mathf.Infinity;

    private string statusTitle = "Status: ";
    private string statusMessage = "waiting for input.";
    private float savedTime;
    private float statusResetDelay = 2;

    public float minXRange;
    public float maxXRange;
    public float minYRange;
    public float maxYRange;
    public float minZRange;
    public float maxZRange;

    [MenuItem("Tools/M.M.A.P")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MattsMassAssetPlacer), true, "Matt's Mass Asset Placer");
    }

    public void OnGUI()
    {
        GUILayout.Label("Welcome to Matt's Mass Asset Placer! To use this set a \nminimum and max range, how many you want, then click spawn!");
        GUILayout.Space(15);
        GUILayout.Label("Range Variables:");
        GUILayout.Space(2.5f);

        minXRange = EditorGUILayout.FloatField("Minimum X Range:", minXRange);
        maxXRange = EditorGUILayout.FloatField("Maximum X Range:", maxXRange);
        minYRange = EditorGUILayout.FloatField("Minimum Y Range:", minYRange);
        maxYRange = EditorGUILayout.FloatField("Maximum Y Range:", maxYRange);
        minZRange = EditorGUILayout.FloatField("Minimum Z Range:", minZRange);
        maxZRange = EditorGUILayout.FloatField("Maximum Z Range:", maxZRange);

        GUILayout.Space(40);
        GUILayout.Label("Controls:");
        GUILayout.Space(2.5f);

        placeOnSurface = GUILayout.Toggle(placeOnSurface, "Place on Surface?");

        count = EditorGUILayout.IntField("Spawn Amount:", count);
        asset = EditorGUILayout.ObjectField("Prefab: ", asset, typeof(Object), false) as GameObject;
        
        if (asset != null)
        { 
            if (GUILayout.Button("Spawn " + count.ToString() + " " + asset.name.ToString() + "s"))
            {
                if (count == 0)
                {
                    Debug.LogWarning("M.M.A.P Count is set to zero, nothing will be spawned.");
                    //GUILayout.Label("M.M.A.P Count is set to zero, nothing will be spawned.");
                }

                for (int i = 0; i < count; i++)
                {
                    GameObject clone = PrefabUtility.InstantiatePrefab(asset) as GameObject;
                    clones.Add(clone);
                    clone.transform.position = new Vector3(Random.Range(minXRange, maxXRange), Random.Range(minYRange, maxYRange), Random.Range(minZRange, maxZRange));
                }
            }
        }

        if (placeOnSurface)
        {
            //surface detection
            RaycastHit hit;
            for (int i = 0; i < clones.Count; i++)
            {
                if (Physics.Raycast(clones[i].transform.position, new Vector3(0,-1,0), out hit, rcRange))
                {
                    if (clones[i] == null)
                        clones.Remove(clones[i]); //if we find a null item in the list, remove that item

                    
                    if (hit.transform.gameObject == null)
                    {
                        Debug.Log("hit nothing");
                        //Remove from the list
                        clones.Remove(clones[i]);
                        //destroy
                        DestroyImmediate(clones[i].gameObject);
                    }
                    else
                    {
                        Debug.Log("hit something");
                        Vector3 GOPos = clones[i].transform.position;
                        GOPos.y -= hit.distance; 
                        clones[i].transform.position = new Vector3(clones[i].transform.position.x, GOPos.y, clones[i].transform.position.z); // remove the hit dist from the y coord
                    }
                }
            }
        }

        if (GUILayout.Button("Randomize range"))
        {
            RandomizeRange();
        }

        if (GUILayout.Button("Save Placements"))
        {
            SavePlacements();
            ResetStatus();
        }

        if (GUILayout.Button("Delete Placements"))
        {
            DeletePlacements();
        }

        GUILayout.Space(40);
        GUILayout.Label(statusTitle + statusMessage);
    }

#region EXTRA FUNCTIONS
    public void SavePlacements()
    {
        clones.Clear();
        Debug.Log("Saving items, removing from master list.. ");
        statusMessage = "Done saving items.";
        savedTime = (float)EditorApplication.timeSinceStartup;
    }

    public void DeletePlacements()
    {
        if (clones.Count > 0)
        {
           while (true)
            {
                for (int i = 0; i < clones.Count; i++)
                {
                    if (clones[i] != null)
                    {
                        Debug.Log("Deleting placed " + clones[i].name);
                        DestroyImmediate(clones[i]);
                        clones.Remove(clones[i]);
                    }
                }
                if (clones.Count == 0)
                {
                    statusMessage = "Deletion operataion complete.";
                    savedTime = (float)EditorApplication.timeSinceStartup;
                    ResetStatus();
                    return;
                }
            }
        }
        else
        {
            statusMessage = "Nothing to delete!";
            ResetStatus();
        }
    }

    public void RandomizeRange()
    {
        statusMessage = "Setting max ranges to something \nbetween 0 - 10000.";

        //minXRange = Random.Range(0, 10000);
        maxXRange = Random.Range(0, 10000);
        //minYRange = Random.Range(0, 10000);
        maxYRange = Random.Range(0, 10000);
        //minZRange = Random.Range(0, 10000);
        maxZRange = Random.Range(0, 10000);

        ResetStatus();
    }

    public void ResetStatus()
    {
        savedTime = (float)EditorApplication.timeSinceStartup;
        Debug.Log(savedTime);

        float calculatedTime = savedTime + statusResetDelay;
        if ((float)EditorApplication.timeSinceStartup >= calculatedTime)
        {
            Debug.Log(EditorApplication.timeSinceStartup);
            Debug.Log(calculatedTime);
            Debug.Log("Resetting status");
            statusMessage = "waiting for input.";
            GUILayout.Label(statusTitle + statusMessage);
        }
    }

#endregion

}