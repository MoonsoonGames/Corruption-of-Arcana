using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class Compass : MonoBehaviour
    {
        
        public float Offset = 90f;
        public RawImage compassHeadings;
        public Transform player;

        float compassUnit;

        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }
        }
        private void Update()
        {
            UpdateCompassRotation();
        }

        void UpdateCompassRotation()
        {
            compassHeadings.uvRect = new Rect((player.localEulerAngles.y / 360f) + Offset, 1f, 1f, 1f);
        }
    }
}
