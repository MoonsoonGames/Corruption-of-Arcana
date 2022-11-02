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
        }
        public void Remove(Character character)
        {
            team.Remove(character);
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
    }
}