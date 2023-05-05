using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com and @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI.Movement
{
    /// <summary>
    /// This is the wander class, it holds all the functions neccesary to get the wander working with the AI. 
    /// With it's main class being Wander()
    /// </summary>
    [RequireComponent(typeof(EnemyAI))]
    public class Wander : MonoBehaviour
    {
        float wanderCooldown;
        float timer;
        public float maxWanderDuration = 10f;
        [SerializeField]
        private float timeLeftTillScriptCleanup;
        EnemyAI aiController;
        private Vector3 homePoint; // The home point for the AI, which it will return to after a set amount of time.

        private Coroutine disableScript;

        // Variable used for the RandomNav Function
        Vector3 newPos;

        private void OnEnable()
        {
            aiController = gameObject.GetComponent<EnemyAI>();
            if (aiController.returnHomeAfterWander == true)
            {
                homePoint = transform.position;
                if (aiController.debugMode == true)
                {
                    GameObject refPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    refPoint.transform.position = homePoint;
                    refPoint.gameObject.name = $"{gameObject.name}HomePoint";
                }

            }

            timeLeftTillScriptCleanup = maxWanderDuration;
            //aiController.currentState = AIState.Wandering;
            disableScript = StartCoroutine(WaitForDelete(maxWanderDuration));
        }

        /// <summary>
        /// Supposed to reset variables on disable, but just gave me more headaches as it seems to keep resetting things regardless
        /// of whether the script is enabled or not.
        /// </summary>
        private void OnDisable()
        {
            StopCoroutine(disableScript);
            StopCoroutine(Cooldown(.1f));
            timer = 0f;
            timeLeftTillScriptCleanup = maxWanderDuration;
            aiController.timer = 0f;
            aiController.currentState = AIState.Nothing;
        }

        private void Update()
        {
            // Might be a good idea to just access the AIControllers timer to avoid sync issues.
            // Start the timer
            timer += Time.deltaTime;

            timeLeftTillScriptCleanup -= (timeLeftTillScriptCleanup > 0) ? Time.deltaTime : 0;
        }

        /// <summary>
        /// This function handles the wandering for the AI.
        /// Uses the Navmesh and picks a point on it to move to. If the point is blocked by something, go to a new point.
        /// This will eventually extend the vision function(or class) to move move out of the wandering state.
        /// </summary>
        /// <param name="timer">Timer, passed in from the controller.</param>
        /// <param name="wanderTimer">How long to wait before moving to a new point, passed in from controller.</param>
        /// <param name="wanderRadius">How far to wander, passed in from controller.</param>
        /// <param name="blocked">Checks to see if the current picked point is blocked, Passed in from controller.</param>
        /// <param name="hit">Used for storing the navmesh location variable, passed in from controller.</param>
        /// <param name="agent">The navmesh agent, passed in from controller.</param>
        public void WanderInRadius(bool blocked, NavMeshHit hit)
        {
            aiController.wanderingCoolDown = wanderCooldown;
            if (timer >= wanderCooldown)
            {
                newPos = RandomWanderPoint(transform.position, aiController.wanderRadius, -1);
                blocked = NavMesh.Raycast(transform.position, newPos, out hit, NavMesh.AllAreas);
                Debug.DrawLine(transform.position, newPos, blocked ? Color.red : Color.green);
                if (!blocked)
                {
                    if (aiController.agent.isOnNavMesh)
                        aiController.agent.SetDestination(newPos);
                    else
                        Debug.LogWarning(gameObject.name + " is not on nav mesh");
                    StartCoroutine(Cooldown(.1f));
                    timer = 0;
                }
                else
                {
                    //Debug.Log($"{data.Agent.name} is blocked! Finding a new point..");
                    timer = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// This void is what allows the AI to wander about the world, it's a little bit rudimentary
        /// but it is for sure good enough for what I need right now. For reference, some of this is
        /// identical to what I used in a previous project *Gimme Gimme*, it worked well enough then, and it
        /// should do the same for now
        ///
        /// <para>The only new parts are the blocked path detection which will make sure the AI doesn't run to a
        /// point it can't get to, causing it to do some unwanted behaviour</para>
        /// </summary>
        /// <param name="origin">Where the point is chosen from(around)</param>
        /// <param name="dist">How far it should pick from around the origin</param>
        /// <param name="layermask">The layermask of things to hit</param>
        /// <returns>nav hit point, which is a point on the navmesh</returns>
        private static Vector3 RandomWanderPoint(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
            return navHit.position;
        }

        IEnumerator Cooldown(float coolDown)
        {
            yield return new WaitForSeconds(coolDown);
            wanderCooldown = UnityEngine.Random.Range(1f, 10f);
        }

        /// <summary>
        /// This coroutine manages the delay that the script will wait before cleaning itself up.
        /// </summary>
        /// <param name="time">How long before the script gets cleaned up</param>
        /// <returns>waits for the alloted time before continuing</returns>
        IEnumerator WaitForDelete(float time)
        {
            yield return new WaitForSeconds(time);
            DisableScript();
        }

        /// <summary>
        /// Disables script, is public so it can be called elsewhere if needed.
        /// </summary>
        public void DisableScript()
        {
            //Debug.Log("ran");
            // after a set amount of time return to the home point
            if (aiController.returnHomeAfterWander == true)
            {
                if (aiController.agent.isOnNavMesh)
                    aiController.agent.SetDestination(homePoint);
                else
                    Debug.LogWarning(gameObject.name + " is not on nav mesh");
            }
            this.enabled = false;
        }
    }
}