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

        private void Start()
        {
            timeline = GameObject.FindObjectOfType<Timeline>();
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
            for (int i = 0; i < team.Count; i++)
            {
                team[i].Invoke("StartTurn", 0.1f);
            }
        }
    }
}