using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necropanda.Interfaces
{
    public class QuitConfirmationInterface : MonoBehaviour
    {
        public GameObject ConfirmationScreen;

        void Start()
        {
            ConfirmationScreen.SetActive(false);
        }

        public void Yes()
        {
            Application.Quit();
        }

        public void No()
        {
            ConfirmationScreen.SetActive(false);
        }
    }
}