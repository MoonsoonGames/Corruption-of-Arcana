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
        public bool checkOnStart = true;
        public QuestHelperFuncions.QuestInstance[] requireStates;

        public E_Operations operation = E_Operations.AND;

        // Start is called before the first frame update
        private void Start()
        {
            if (checkOnStart)
                CheckProgress();
        }

        public void CheckProgress()
        {
            bool or = false;
            bool and = true;

            foreach (var item in requireStates)
            {
                if (item.Available())
                    or = true;
                else
                    and = false;
            }

            switch (operation)
            {
                case (E_Operations.AND):
                    gameObject.SetActive(and);
                    break;
                case (E_Operations.OR):
                    gameObject.SetActive(or);
                    break;
            }
        }
    }

    public enum E_Operations
    {
        OR, AND
    }
}
