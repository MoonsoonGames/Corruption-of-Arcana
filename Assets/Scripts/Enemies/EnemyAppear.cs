using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.AI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EnemyAppear : MonoBehaviour, IInteractable, ICancelInteractable
    {
        public GameObject art;
        public Object fx;
        public LayerMask layerMask;
        bool active = false;
        EnemyAI aiScript;

        GameObject player;

        public bool activateOnStart = false;

        float height;

        Vector3 sightPos;
        Vector3 targetDirection;

        private void Start()
        {
            art.SetActive(activateOnStart);
            aiScript = GetComponent<EnemyAI>();

            height = GetComponent<CapsuleCollider>().height;

            if (activateOnStart)
            {
                fx = null;
                active = true;
                aiScript.ActivateAI(player);
            }

        }

        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject playerRef)
        {
            player = playerRef;
            Interactable.Interactable loadInteract = GetComponentInChildren<LoadCombat>().GetComponent<Interactable.Interactable>();
            loadInteract.UnlockInteraction();
        }

        public void CancelInteraction(GameObject playerRef)
        {
            if (playerRef == player)
            {
                Debug.Log("Cancel interaction");

                //player = null;
            }
        }

        private void Update()
        {
            if (player != null)
            {
                sightPos = gameObject.transform.position;
                sightPos.y = gameObject.transform.position.y + height;

                targetDirection = player.transform.position - sightPos;
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(sightPos, targetDirection, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.gameObject == player)
                    {
                        if (!active)
                        {
                            //Debug.Log("Interacted - Unearth and activate AI");
                            //Unearth and activate AI
                            art.SetActive(true);
                            aiScript.ActivateAI(player);
                            if (fx != null)
                            {
                                Instantiate(fx, this.gameObject.transform);
                                fx = null;
                            }

                            active = true;
                        }
                        else
                        {
                            //Already active
                        }
                    }

                    Debug.DrawRay(sightPos, targetDirection, Color.yellow);
                }
                else
                {
                    Debug.DrawRay(sightPos, targetDirection, Color.white);
                    Debug.Log("Did not Hit " + targetDirection);
                }
            }
        }
    }
}
