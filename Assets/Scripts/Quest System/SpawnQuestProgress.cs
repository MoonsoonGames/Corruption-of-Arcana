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
    public class SpawnQuestProgress : MonoBehaviour
    {
        public Vector3[] positions;
        public QuestHelperFuncions.QuestInstance[] requireStates;

        // Start is called before the first frame update
        void Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }

            for (int i = 0; i < requireStates.Length; i++)
            {
                if (requireStates[i].Available())
                {
                    transform.position = positions[i];
                    //Debug.Log(positions[i] + " || " + transform.position);
                    
                    if (agent != null)
                    {
                        agent.enabled = true;
                        agent.SetDestination(positions[i]);
                    }

                    //return;
                }
            }
        }

        [ContextMenu("Debug Position")]
        public void DebugPosition()
        {
            Debug.Log(transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;

            for (int i = 0; i < positions.Length; i++)
            {

                Gizmos.DrawSphere(positions[i], 1);
            }
        }
    }
}
