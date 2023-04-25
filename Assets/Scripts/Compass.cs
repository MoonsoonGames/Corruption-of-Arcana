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
        public RawImage compassHeadings;
        public GameObject player;

        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        private void Update()
        {
            UpdateCompassRotation();
        }

        void UpdateCompassRotation()
        {
            compassHeadings.uvRect = new Rect((player.transform.localEulerAngles.y) + 90f / 360f, 0f, 1f, 1f);
        }
    }
}
