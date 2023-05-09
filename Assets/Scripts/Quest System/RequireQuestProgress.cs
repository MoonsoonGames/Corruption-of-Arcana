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
        protected virtual void Start()
        {
            if (checkOnStart)
                CheckProgress();
        }

        public virtual void CheckProgress()
        {
            if (requireStates.Length == 0)
            {
                SetMarker(false);
                return;
            }

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
                    SetMarker(and);
                    break;
                case (E_Operations.OR):
                    SetMarker(or);
                    break;
            }
        }

        protected virtual void SetMarker(bool active)
        {
            gameObject.SetActive(active);
        }
    }

    public enum E_Operations
    {
        OR, AND
    }
}
