using UnityEngine;
using FMODUnity;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Audio
{
    public class PlaySoundOnKeyPress : MonoBehaviour, IInteractable
    {
        public EventReference sound;

        public void Interacted(GameObject player)
        {
            RuntimeManager.PlayOneShot(sound);
        }
    }
}
