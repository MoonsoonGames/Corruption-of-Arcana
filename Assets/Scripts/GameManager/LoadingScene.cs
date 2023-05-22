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

        public bool loadOnStart = true;

        // Start is called before the first frame update
        void Start()
        {
            if (loadOnStart)
                SceneLoaded();
            Singleton();
        }

        public static int loadPos;
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

        public void LoadScene(E_Scenes scene, E_Scenes lastScene, int newLoadPos)
        {
            if (scene == E_Scenes.Navigation)
            {
                navScene = lastScene;
            }

            loadPos = newLoadPos;

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

        public void LoadLastScene(E_Scenes lastScene, int loadPos)
        {
            E_Scenes scene = LoadCombatManager.instance.lastScene;

            //Save current player position if applicable
            LoadScene(scene, lastScene, loadPos);
        }

        public void SceneLoaded()
        {
            SaveManager.instance.LoadAllData();
        }
    }
}