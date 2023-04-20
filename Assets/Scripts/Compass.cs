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
        public float offset = 22.5f;
        public RawImage compassHeadings;
        public Transform player;


        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            }
        }
        public void Update()
        {
            compassHeadings.uvRect = new Rect((player.localEulerAngles.y / 360f) + offset, 1f, 1f, 1f);
        }
    }
}
