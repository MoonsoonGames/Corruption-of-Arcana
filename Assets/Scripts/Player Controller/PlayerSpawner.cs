using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class PlayerSpawner : MonoBehaviour
    {
        public Object playerRef;

        // Start is called before the first frame update
        void Start()
        {
            if (playerRef == null) { return; }

            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;

            string currentSceneString = SceneManager.GetActiveScene().name;
            E_Scenes currentScene = HelperFunctions.StringToSceneEnum(currentSceneString);

            if (LoadCombatManager.instance != null)
            {
                if (LoadCombatManager.instance.lastScene != E_Scenes.Null)
                {
                    if (currentScene == LoadCombatManager.instance.lastScene)
                    {
                        Debug.Log("1" + transform.position + " || " + LoadCombatManager.instance.lastPos);
                        spawnPos = LoadCombatManager.instance.lastPos;
                        spawnRot = LoadCombatManager.instance.lastRot;
                        Debug.Log("2" + transform.position + " || " + LoadCombatManager.instance.lastPos);
                    }
                }
            }

            GameObject player = Instantiate(playerRef, spawnPos, spawnRot) as GameObject;

            //set up references
            Debug.Log(player.name);
        }
    }
}
