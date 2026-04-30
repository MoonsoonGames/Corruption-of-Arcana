using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Player;
using Fungus;

namespace Necropanda.Interfaces
{
    public class PauseInterface : MonoBehaviour
    {
        //public GameObject ConfirmationScreen;
        public GameObject settingsScreen;
        public GameObject mainHUD;
        public GameObject pauseMenu;
        public GameObject savedText;
        public GameObject creditsScreen;
        //public GameObject AchievementScreen;

        public static PauseInterface instance;


        private void Start()
        {

        }

        public void Settings()
        {
            pauseMenu.SetActive(false);
            settingsScreen.SetActive(true);
        }

        public void Credits()
        {
            pauseMenu.SetActive(false);
            creditsScreen.SetActive(true);
        }

        public void SaveGame()
        {
            //Maybe have a visual indicator for this
            SaveManager.instance.SaveAllData();
            StartCoroutine(IDelaySaveText());
        }

        public void QuitGame()
        {
            //ConfirmationScreen.SetActive(true);
            LoadingScene.instance.LoadScene(E_Scenes.SplashScreen, E_Scenes.Null, 0);
        }

        IEnumerator IDelaySaveText(float delay = 2)
        {
            savedText.SetActive(true);
            yield return new WaitForSecondsRealtime(delay);
            savedText.SetActive(false);
        }
    }
}