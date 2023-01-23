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
    public class LoadCombat : MonoBehaviour, IInteractable
    {
        public string ID;
        public int questState;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            string sceneString = SceneManager.GetActiveScene().name;
            E_Scenes lastScene = HelperFunctions.StringToSceneEnum(sceneString);
            LoadCombatManager.instance.questStateUponCombatVictory = questState;
            LoadCombatManager.instance.LoadCombat(player, lastScene);
        }
    }
}
