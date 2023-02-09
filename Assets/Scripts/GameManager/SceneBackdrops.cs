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
    public class SceneBackdrops : MonoBehaviour
    {
        public Sprite[] backdrops;

        public static SceneBackdrops instance;

        private void Start()
        {
            instance = this;
        }

        public void SetBackdrop()
        {
            if (backdrops.Length > 0)
            {
                Sprite backdrop = backdrops[Random.Range(0, backdrops.Length)];
                LoadCombatManager.instance.backdrop = backdrop;
            }
        }
    }
}
