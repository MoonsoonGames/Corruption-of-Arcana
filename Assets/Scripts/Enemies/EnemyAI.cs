using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EnemyAI : MonoBehaviour
    {
        bool active = false;
        GameObject player;
        NavMeshAgent agent;

        public void ActivateAI(GameObject playerRef)
        {
            agent = GetComponent<NavMeshAgent>();
            player = playerRef;
            active = true;
            Debug.Log("Activate AI");
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }
}