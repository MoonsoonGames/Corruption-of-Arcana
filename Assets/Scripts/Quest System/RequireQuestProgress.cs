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
    public class RequireQuestProgress : MonoBehaviour
    {
        public QuestHelperFuncions.QuestInstance[] requireStates;

        // Start is called before the first frame update
        void Start()
        {
            bool available = false;

            foreach (var item in requireStates)
            {
                if (item.Available())
                    available = true;
            }

            gameObject.SetActive(available);
        }
    }
}
