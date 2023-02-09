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
    public class MenuHandler : MonoBehaviour
    {
        public GameObject Page1;
        public GameObject Page2;
        // Start is called before the first frame update
        void Start()
        {
            Page1.SetActive(true);
            Page2.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void NextPage()
        {
            Page1.SetActive(false);
            Page2.SetActive(true);
        }

        public void LastPage()
        {
            Page1.SetActive(true);
            Page2.SetActive(false);
        }
    }
}
