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
    public class Collectable : MonoBehaviour, IInteractable
    {
        public void Interacted(GameObject player)
        {
            Destroy(this.gameObject);
        }
    }
}
