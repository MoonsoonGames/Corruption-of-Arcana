using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            for (int i = 0; i < requireStates.Length; i++)
            {
                if (requireStates[i].Available())
                {
                    gameObject.transform.position = positions[i];
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
