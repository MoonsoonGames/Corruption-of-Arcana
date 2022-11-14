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
    public class DestroyDelay : MonoBehaviour
    {
        public float delay;

        private void Start()
        {
            Invoke("DestroyObject", delay);
        }

        void DestroyObject()
        {
            Destroy(this.gameObject);
        }
    }
}
