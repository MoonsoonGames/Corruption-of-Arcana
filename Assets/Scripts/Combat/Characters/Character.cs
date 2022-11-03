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
        #region Setup

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

        #endregion

        #region Taking Turn

        public virtual CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            //Overwritten by children
            return new CombatHelperFunctions.SpellUtility();
        }

        public virtual void StartTurn()
        {
            
        }

        #endregion

        #region Health Checks

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

        #endregion

        #region Statuses

        //Negative Statuses
        protected bool banish, charm, silence, stun, curse;

        public void ApplyStatus(bool apply, E_Statuses status)
        {
            switch (status)
            {
                //Negative Effects
                case E_Statuses.Banish:
                    banish = apply;
                    break;
                case E_Statuses.Charm:
                    charm = apply;
                    break;
                case E_Statuses.Silence:
                    silence = apply;
                    break;
                case E_Statuses.Stun:
                    stun = apply;
                    break;
                case E_Statuses.Curse:
                    curse = apply;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}