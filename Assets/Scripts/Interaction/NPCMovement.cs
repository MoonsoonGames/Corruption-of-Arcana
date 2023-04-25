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

        Animator animator;
        public Vector2 animatorDelay = new Vector2(0, 1);

        private void Start()
        {
            animator = GetComponent<Animator>();

            animator.enabled = false;
            animator.SetBool("Moving", false);
            Invoke("Setup", Random.Range(animatorDelay.x, animatorDelay.y));
        }

        void Setup()
        {
            animator.enabled = true;
        }

        public void SetDestination(Vector3 position)
        {
            Debug.Log("moving to " + position);
            StartCoroutine(ISetDestination(position, defaultDelay));
        }

        public IEnumerator ISetDestination(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);

            agent.SetDestination(position);
        }

        private void Update()
        {
            bool moving = 
                (
                agent.velocity.x > 0.5f || agent.velocity.x < -0.5f ||
                agent.velocity.y > 0.5f || agent.velocity.y < -0.5f ||
                agent.velocity.z > 0.5f || agent.velocity.z < -0.5f
                );

            animator.SetBool("Moving", moving);
        }
    }
}