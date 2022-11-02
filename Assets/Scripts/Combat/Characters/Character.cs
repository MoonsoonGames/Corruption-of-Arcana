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
        public CharacterStats stats;
        protected TeamManager teamManager; public TeamManager GetManager() { return teamManager; }
        protected CharacterHealth health; public CharacterHealth GetHealth() { return health; }

        public EnemySpawner spawner;

        protected virtual void Start()
        {
            health = GetComponent<CharacterHealth>();

            teamManager = GetComponentInParent<TeamManager>();
            teamManager.Add(this);
        }

        public virtual CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            //Overwritten by children
            return new CombatHelperFunctions.SpellUtility();
        }

        public virtual void StartTurn()
        {
            
        }

        public void CheckHealth()
        {
            if (health.GetHealth() < 1)
            {
                StartCoroutine(IDelayDeath(0.01f));
            }
            else
            {
                //Debug.Log(characterName + " has " + health.GetHealth() + " health left");
            }
        }

        public IEnumerator IDelayDeath(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Debug.Log(characterName + " Should be killed");
            health.PlayDeathSound();
            CombatManager.instance.CharacterDied(this);
            teamManager.team.Remove(this);
            if (spawner != null)
            {
                spawner.filled = false;
            }
            Destroy(gameObject);
        }
    }
}