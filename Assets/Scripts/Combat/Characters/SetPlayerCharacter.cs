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
    public class SetPlayerCharacter : MonoBehaviour
    {
        public Character player;
        public CharacterStats defaultStats;

        // Start is called before the first frame update
        void Start()
        {
            if (LoadCombatManager.instance != null)
                player.stats = LoadCombatManager.instance.character;
            else
                player.stats = defaultStats;
        }
    }
}
