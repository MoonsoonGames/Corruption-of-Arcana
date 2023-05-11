using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Necropanda.Player;
using Necropanda.Interfaces;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class MapSelector : MonoBehaviour
    {
        public Sprite[] maps;
        public Image currentMap;

        [Space]
        public KeyCode mapKey = KeyCode.M;

        [Space]
        public PlayerController playerController;
        public HUDInterface hudInterface;

        // Start is called before the first frame update
        void Start()
        {
            currentMap.enabled = false;

        }

        private void Update()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            hudInterface = FindObjectOfType<HUDInterface>();
            CheckInput();
        }

        public void CheckInput()
        {
            if (Input.GetKeyDown(mapKey))
            {
                currentMap.enabled = !currentMap.enabled;
                playerController.paused = !playerController.paused;
                playerController.canMove = !playerController.canMove;
                hudInterface.gameIsPaused = !hudInterface.gameIsPaused;

                UpdateMapImage();
            }
        }

        public void CloseMap()
        {
            currentMap.enabled = false;
        }

        public void UpdateMapImage()
        {
            Scene scene = SceneManager.GetActiveScene();

            switch (scene.name)
            {
                case "Thoth":
                    currentMap.sprite = maps[1];
                    break;
                case "EastForest":
                    currentMap.sprite = maps[2];
                    break;
                case "Cave":
                    currentMap.sprite = maps[3];
                    break;

                default:
                    // If the scene isn't recognized, set the image to the main map.
                    currentMap.sprite = maps[0];
                    break;

            }
        }
    }
}
