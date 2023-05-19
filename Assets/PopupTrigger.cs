using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Necropanda
{

    public class PopupTrigger : MonoBehaviour
    {
        public ScriptableObject[] LocationPopups;
        public int PopupNum;
        public Image PopupArt;
        public GameObject Popup;
        BoxCollider col;

        // Start is called before the first frame update

        public void Start()
        {
            col = GetComponent<BoxCollider>();
        }
        public void SetLocation(Location_Popups loc)
        {

            PopupArt.sprite = loc.Artwork;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Location_Popups loc = (Location_Popups)LocationPopups[PopupNum]; 
                SetLocation(loc);
                Popup.SetActive(true);
                WaitTime(4);
                Popup.SetActive(false);
            }
        }

        IEnumerator WaitTime(int Time)
        {
            yield return new WaitForSecondsRealtime(Time);
        }
    }
}