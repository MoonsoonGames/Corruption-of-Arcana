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
    public class EnemyMapMovement : MonoBehaviour
    {
        public GameObject target;
        public NavMeshAgent agent;

        private void Update()
        {
            if (target != null)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }
}
