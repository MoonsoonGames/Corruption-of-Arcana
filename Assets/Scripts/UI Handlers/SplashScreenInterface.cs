using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Necropanda.Interfaces
{
    public class SplashScreenInterface : MonoBehaviour
    {
        public GameObject SettingsMenu;
        public E_Scenes initialScene;

        private void Start()
        {
            Invoke("DelaySaveBaseData", 0.1f);
        }

        void DelaySaveBaseData()
        {
            SaveManager.instance.SaveAllBaseData();
        }

        public void NewGame()
        {
            SaveManager.instance.LoadAllBaseData();
            SaveManager.instance.SaveAllData();
            //Reset loadsettings/progress
            if (initialScene == E_Scenes.Null)
            {
                Debug.LogWarning("no initial scene");
            }

            if (LoadingScene.instance == null)
                Debug.LogWarning("TF is this null");

            LoadingScene.instance.LoadScene(initialScene, E_Scenes.Null, false);
            //load game
        }

        public void ArenaMode()
        {
            LoadingScene.instance.LoadScene(E_Scenes.ArenaMode, E_Scenes.Null, false);
        }

        public void LoadGame()
        {
            //locate save file
            //set load settings/progress
            //load game

            SaveManager.instance.LoadAllData();
            //Reset loadsettings/progress

            if (LoadingScene.instance.loadScene != E_Scenes.Null)
                LoadingScene.instance.LoadScene(LoadingScene.instance.loadScene, E_Scenes.Null, true);
            else
                NewGame();
        }

        public void Settings()
        {
            SettingsMenu.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}