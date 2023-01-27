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
    public class NPCMovement : MonoBehaviour
    {
        public NavMeshAgent agent;

        public float defaultDelay = 0.5f;

        public void SetDestination(Vector3 position)
        {
            StartCoroutine(ISetDestination(position, defaultDelay));
        }

        public IEnumerator ISetDestination(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);

            agent.SetDestination(position);
        }
    }
}