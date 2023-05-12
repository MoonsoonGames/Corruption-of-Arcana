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
        public GameObject currentMap;
        private Image currentMapImage;

        [Space]
        public KeyCode mapKey = KeyCode.M;

        [Space]
        public PlayerController playerController;
        public HUDInterface hudInterface;

        // Start is called before the first frame update
        void Start()
        {
            currentMap.SetActive(false);
            currentMapImage = currentMap.GetComponent<Image>();
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
                ToggleMap();
            }
        }

        public void ToggleMap()
        {
            currentMap.SetActive(!currentMap.activeInHierarchy);
            SetVariables();
            UpdateMapImage();
        }

        public void UpdateMapImage()
        {
            Scene scene = SceneManager.GetActiveScene();

            switch (scene.name)
            {
                case "Thoth":
                    currentMapImage.sprite = maps[1];
                    break;
                case "EastForest":
                    currentMapImage.sprite = maps[2];
                    break;
                case "Cave":
                    currentMapImage.sprite = maps[3];
                    break;

                default:
                    // If the scene isn't recognized, set the image to the main map.
                    currentMapImage.sprite = maps[0];
                    break;

            }
        }

        private void SetVariables()
        {
            playerController.paused = !playerController.paused;
            playerController.canMove = !playerController.canMove;
            hudInterface.gameIsPaused = !hudInterface.gameIsPaused;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = !Cursor.visible;


            // Set the timescale back to 1 - this is because if we don't have it, after close the map from the
            // inventory the games timescale isn't set properly and this is easier for me.
            Time.timeScale = 1;
        }
    }
}
