using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    /// <summary>
    /// Temporary script for loading tiertarock
    /// 
    /// could be converted to load any scene. Would just need a few
    /// tweaks.
    /// </summary>
    public class LoadScene : MonoBehaviour, IInteractable
    {
        public E_Scenes scene;
        public int loadEntrance;
        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        // Start is called before the first frame update
        public void Interacted(GameObject player)
        {
            LoadDefaultScene();
        }

        public void LoadDefaultScene()
        {
            string sceneString = SceneManager.GetActiveScene().name;
            E_Scenes lastScene = HelperFunctions.StringToSceneEnum(sceneString);

            LoadingScene.instance.LoadScene(scene, lastScene, 0);
        }

        public void LoadSetScene(E_Scenes scene)
        {
            string sceneString = SceneManager.GetActiveScene().name;
            E_Scenes lastScene = HelperFunctions.StringToSceneEnum(sceneString);

            LoadingScene.instance.LoadScene(scene, lastScene, 0);
        }
    }
}
