using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <OtisHull/NOOT-TEC>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Audio
{
    public class VcaController : MonoBehaviour
    {
        private FMOD.Studio.VCA VcaSliderController;
        public string VcaName;

        private Slider slider;
        // Start is called before the first frame update
        void Start()
        {
            VcaSliderController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
            slider = GetComponent<Slider>();
        }

        public void SetVolume(float volume)
        {
            VcaSliderController.setVolume(volume);
        }
    }
}
