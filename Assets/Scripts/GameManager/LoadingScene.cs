using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class LoadingScene : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static LoadingScene instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;
                
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SceneLoaded();
            Singleton();
        }

        public bool loadLastPos;
        public E_Scenes navScene;
        public E_Scenes loadScene;

        List<E_Scenes> savableScenes = new List<E_Scenes>
        {
            E_Scenes.Thoth,
            E_Scenes.EastForest,
            E_Scenes.Cave
        };

        public void SaveScene()
        {
            string sceneString = SceneManager.GetActiveScene().name;

            E_Scenes sceneEnum = HelperFunctions.StringToSceneEnum(sceneString);

            if (savableScenes.Contains(sceneEnum))
                loadScene = sceneEnum;
        }

        public void LoadScene(E_Scenes scene, E_Scenes lastScene, bool loadLastPos)
        {
            if (scene == E_Scenes.Navigation)
            {
                navScene = lastScene;
            }

            this.loadLastPos = loadLastPos;
            if (lastScene != E_Scenes.Null)
            {
                LoadCombatManager.instance.lastScene = lastScene;
            }

            if (SceneBackdrops.instance != null)
            {
                SceneBackdrops.instance.SetBackdrop();
            }

            SaveManager.instance.SaveAllData();

            //Save current player position if applicable
            Time.timeScale = 1;
            SceneManager.LoadScene(scene.ToString());
        }

        public void LoadLastScene(E_Scenes lastScene, bool loadLastPos)
        {
            E_Scenes scene = LoadCombatManager.instance.lastScene;

            //Save current player position if applicable
            LoadScene(scene, lastScene, loadLastPos);
        }

        public void SceneLoaded()
        {
            SaveManager.instance.LoadAllData();
        }
    }
}