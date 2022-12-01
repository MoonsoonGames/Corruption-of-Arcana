using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class StopEffect : MonoBehaviour
    {
        public float delay;
        public VisualEffect effect;

        private void Start()
        {
            Invoke("DestroyObject", delay);
        }

        void DestroyObject()
        {
            effect.Stop();
        }
    }
}