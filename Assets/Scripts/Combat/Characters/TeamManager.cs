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
    public class TeamManager : MonoBehaviour
    {
        protected Timeline timeline;

        public List<Character> team = new List<Character>();
        [HideInInspector]
        public List<Character> opposingTeam = new List<Character>();

        protected virtual void Start()
        {
            timeline = GameObject.FindObjectOfType<Timeline>();
            Setup();
        }

        protected virtual void Setup()
        {

        }

        public void Add(Character character)
        {
            team.Add(character);
            RecalculateAllTeams();
        }
        public void Remove(Character character)
        {
            team.Remove(character);
            RecalculateAllTeams();
        }

        void RecalculateAllTeams()
        {
            foreach (TeamManager teamManager in GameObject.FindObjectsOfType<TeamManager>())
            {
                teamManager.RecalculateTeams();
            }
        }

        public void RecalculateTeams()
        {
            opposingTeam = new List<Character>();

            foreach (Character character in GameObject.FindObjectsOfType<Character>())
            {
                if (team.Contains(character) == false)
                {
                    opposingTeam.Add(character);
                }
            }
        }

        public virtual void StartTurn()
        {
            CheckKilled();
            ActivateTurns();
        }

        public void CheckKilled()
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].CheckHealth();
            }
        }
        
        public virtual void ActivateTurns()
        {
            for (int i = 0; i < team.Count; i++)
            {
                team[i].StartTurn();
            }
        }

        public void AddSpellInstance(CombatHelperFunctions.SpellInstance newSpellInstance)
        {
            timeline.AddSpellInstance(newSpellInstance);
        }
    }
}