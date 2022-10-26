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
    public class EnemyAppear : MonoBehaviour, IInteractable
    {
        public GameObject art;
        public Object fx;
        bool active = false;
        EnemyAI aiScript;

        private void Start()
        {
            art.SetActive(false);
            aiScript = GetComponent<EnemyAI>();
        }

        public void Interacted(GameObject player)
        {
            if (!active)
            {
                Debug.Log("Interacted - Unearth and activate AI");
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

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.name + " has left collision");

                Deactivate();
            }
        }

        void Deactivate()
        {
            if (active)
            {
                Debug.Log("Deactivate AI");
                //Unearth and activate AI
                //art.SetActive(true);
                aiScript.DeactivateAI();

                active = false;
            }
            else
            {
                //Already active
            }
        }
    }
}
