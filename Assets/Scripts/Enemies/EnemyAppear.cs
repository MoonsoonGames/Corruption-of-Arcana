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
                }

                active = true;
            }
            else
            {
                //Already active
            }
        }
    }
}
