using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class LoadTiertarock : MonoBehaviour, IInteractable
    {
        public E_Scenes scene;

        // Start is called before the first frame update
        public void Interacted(GameObject player)
        {
            LoadScene();
        }

        public void LoadScene()
        {
            LoadingScene.instance.LoadScene(scene);
        }
    }
}
