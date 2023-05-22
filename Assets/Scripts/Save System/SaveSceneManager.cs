using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SaveSceneManager : MonoBehaviour
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled

        #region Singleton
        //Code from last year

        public static SaveSceneManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                //Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
            SaveManager.instance.saveAllData += SaveSceneData;
            SaveManager.instance.loadAllData += LoadSceneData;
            SaveManager.instance.resetAllData += ResetSceneData;
        }

        [ContextMenu("Save Quest Data")]
        public void SaveSceneData()
        {
            //SaveScene
            if (LoadingScene.instance != null)
                LoadingScene.instance.SaveScene();

            // Save scene name, pos, rot
            SavingLoading.instance.Save();
        }

        [ContextMenu("Load Scene Data")]
        public void LoadSceneData()
        {
            //LoadingScene.instance.loadScene = GetData();
            SavingLoading.instance.Load();

        }

        [ContextMenu("Load Base Quest Data")]
        public void ResetSceneData()
        {
            //LoadingScene.instance.loadScene = GetData();
            SavingLoading.instance.ResetData();
        }

        public E_Scenes sceneToLoad;
        public Vector3 playerPos;
    }
}
