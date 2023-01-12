using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EndCombat : MonoBehaviour
    {
        //public E_Scenes victoryScene;
        //public E_Scenes defeatScene;

        public void LoadVictoryScene()
        {
            DeckManager.instance.ResetDecks();
            LoadingScene.instance.LoadLastScene(E_Scenes.Null);
        }

        public void LoadDefeatScene()
        {
            DeckManager.instance.ResetDecks();
            LoadingScene.instance.LoadLastScene(E_Scenes.Null);
        }
    }
}
