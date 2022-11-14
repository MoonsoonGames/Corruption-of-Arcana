using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EnemyAI : MonoBehaviour
    {
        bool active = false; public bool GetActive() { return active; }
        GameObject player;
        NavMeshAgent agent;
        Vector3 startPos;

        public Object enemyObject;
        public bool boss;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            startPos = transform.position;
        }

        public void ActivateAI(GameObject playerRef)
        {
            player = playerRef;
            active = true;
            Debug.Log("Activate AI");
        }

        public void DeactivateAI()
        {
            player = null;
            active = false;
            Debug.Log("Deactivate AI");
            agent.SetDestination(startPos);
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