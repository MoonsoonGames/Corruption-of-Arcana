using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

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
            SpawnPlayer();
            Destroy(this.gameObject);
        }

        void SpawnPlayer()
        {
            if (playerRef == null) { return; }

            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;

            string currentSceneString = SceneManager.GetActiveScene().name;
            E_Scenes currentScene = HelperFunctions.StringToSceneEnum(currentSceneString);

            if (LoadingScene.instance.loadLastPos && LoadCombatManager.instance != null)
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
            Player.PlayerController controller = player.GetComponent<Player.PlayerController>();

            Interfaces.HUDInterface hud = GameObject.FindObjectOfType<Interfaces.HUDInterface>();
            hud.player = controller;

            //Setting Up camera stack
            Camera cam = player.GetComponentInChildren<Camera>();
            Camera.SetupCurrent(cam);
            UniversalAdditionalCameraData cameraData = cam.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(GameObject.Find("UICamera").GetComponent<Camera>());
            LoadCombatManager.instance.mainCam = cam;
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
