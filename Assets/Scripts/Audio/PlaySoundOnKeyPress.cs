using UnityEngine;
using FMODUnity;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Audio
{
    public class PlaySoundOnKeyPress : MonoBehaviour
    {
        public EventReference sound;

        private void OnTriggerStay(Collider other) {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RuntimeManager.PlayOneShot(sound);
            }
        }
    }
}
