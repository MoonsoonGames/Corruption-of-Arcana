using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com / @mattordev (remap func)
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Old
{
    public class DropShadow : MonoBehaviour
    {
        public GameObject dropShadow;

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(transform.position, Vector3.down * 1000f, Color.magenta);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1000f, 1 << LayerMask.NameToLayer("Environment"));

            if (hit.collider != null)
            {
                Debug.Log("Blocked");
                dropShadow.transform.position = hit.transform.position;
            }
        }
    }
}

