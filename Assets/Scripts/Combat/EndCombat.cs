using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EndCombat : MonoBehaviour
    {
        public E_Scenes victoryScene;
        public E_Scenes defeatScene;

        public void LoadVictoryScene()
        {
            LoadingScene.instance.LoadScene(victoryScene);
        }

        public void LoadDefeatScene()
        {
            LoadingScene.instance.LoadScene(defeatScene);
        }
    }
}
