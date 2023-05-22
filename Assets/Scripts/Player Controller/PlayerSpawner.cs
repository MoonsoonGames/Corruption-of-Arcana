using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using Cinemachine;

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
        public Vector3[] spawnPositions;
        public Vector3[] spawnRotations;

        CinemachineBrain cmBrain;
        CinemachineFreeLook freeLook;

        // Start is called before the first frame update
        void Start()
        {
            cmBrain = GetComponentInChildren<CinemachineBrain>();
            freeLook = GetComponentInChildren<CinemachineFreeLook>();
            SpawnPlayer();
            //Destroy(this.gameObject);
        }

        void SpawnPlayer()
        {
            if (playerRef == null) { return; }

            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;

            string currentSceneString = SceneManager.GetActiveScene().name;
            E_Scenes currentScene = HelperFunctions.StringToSceneEnum(currentSceneString);

            if (LoadingScene.instance != null)
            {
                if (LoadingScene.loadPos >= 0 && LoadingScene.loadPos < spawnPositions.Length)
                {
                    //Debug.Log("Loadposition " + LoadingScene.loadPos);
                    spawnPos = spawnPositions[LoadingScene.loadPos];
                    spawnRot = Quaternion.Euler(spawnRotations[LoadingScene.loadPos]);
                }
                else if (LoadingScene.instance.loadScene != E_Scenes.Null)
                {
                    if (!HelperFunctions.AlmostEqualVector3(LoadCombatManager.instance.lastPos, Vector3.negativeInfinity, 100f, new Vector3(0, 0, 0)))
                    {
                        if (currentScene == LoadingScene.instance.loadScene)
                        {
                            //Debug.Log("Loadposition last pos");
                            //Debug.Log("1" + transform.position + " || " + LoadCombatManager.instance.lastPos);
                            spawnPos = LoadCombatManager.instance.lastPos;
                            spawnRot = LoadCombatManager.instance.lastRot;
                            //Debug.Log("2" + transform.position + " || " + LoadCombatManager.instance.lastPos);
                        }
                        else
                        {
                            //Debug.Log("loadposition default pos");
                        }
                    }
                }
            }
            
            transform.position = spawnPos;
            transform.rotation = spawnRot;

            GameObject player = Instantiate(playerRef, spawnPos, spawnRot) as GameObject;
            Player.PlayerController controller = player.GetComponent<Player.PlayerController>();

            Interfaces.HUDInterface hud = GameObject.FindObjectOfType<Interfaces.HUDInterface>();
            hud.player = controller;


            //Setting up cam free look
            //Vector3 localPos = freeLook.transform.localPosition;

            //cmBrain.gameObject.transform.parent = controller.gameObject.transform;
            //freeLook.gameObject.transform.parent = controller.gameObject.transform;

            //rotation should look at player

            Vector3 dir = spawnRot.eulerAngles.normalized * 3;

            Vector3 camPos = spawnPos + dir;

            Quaternion camRot = Quaternion.FromToRotation(camPos, spawnPos);

            freeLook.ForceCameraPosition(spawnPos, camRot);

            //freeLook.transform.localPosition = localPos;

            controller.cam = Camera.main;
            controller.cmBrain = cmBrain;

            freeLook.Follow = controller.lookAtTarget.transform;
            freeLook.ResolveFollow(freeLook.Follow);
            freeLook.LookAt = controller.lookAtTarget.transform;
            freeLook.ResolveLookAt(freeLook.LookAt);

            freeLook.ForceCameraPosition(spawnPos, camRot);
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.5f);

            for (int i = 0; i < spawnPositions.Length; i++)
            {
                //Vector3 end = spawnRotations[i] * spawnPositions[i];
                //end = end * 2f;
                //Gizmos.DrawLine(spawnPositions[i], end);
                Gizmos.DrawSphere(spawnPositions[i], 1);
            }
        }
    }
}
