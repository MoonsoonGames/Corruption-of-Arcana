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
    public class MoveObjectSwitch : MonoBehaviour, IInteractable
    {
        public MoveObject[] objects;

        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            if (objects.Length > 0)
            {
                foreach (MoveObject level in objects)
                {
                    if (setSpecificPosInt)
                        level.AdjustPosition(positionCount);
                    else if (setSpecificPosVector3)
                        level.AdjustPosition(targetPos);
                    else
                        level.AdjustPosition();
                }
            }
            else
            {
                //There is no object component
                Debug.LogWarning("No object components have been set");
            }
        }

        public bool setSpecificPosInt = false;
        public int positionCount;
        public bool setSpecificPosVector3 = false;
        public Vector3 targetPos;
    }
}