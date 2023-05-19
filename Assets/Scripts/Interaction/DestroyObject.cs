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
    public class DestroyObject : MonoBehaviour, IInteractable
    {
        public string ID;

        public void SetID(string newID)
        {
            ID = newID;
        }

        public GameObject destroy;

        public void Interacted(GameObject player)
        {
            Destroy(destroy);
        }
    }
}
