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
    public class Character : MonoBehaviour
    {
        public string characterName;
        public Color timelineColor = new Color(0, 0, 0, 255); //Sets up alpha
        protected TeamManager teamManager; public TeamManager GetManager() { return teamManager; }
        protected CharacterHealth health; public CharacterHealth GetHealth() { return health; }

        private void Start()
        {
            health = GetComponent<CharacterHealth>();

            teamManager = GetComponentInParent<TeamManager>();
            teamManager.Add(this);
        }

        public virtual void Die()
        {
            CombatManager.instance.CharacterDied(this);
            //teamManager.Remove(this);
            Destroy(gameObject);
        }

        public virtual Spell PrepareSpell()
        {
            //Overwritten by children
            return null;
        }
    }
}