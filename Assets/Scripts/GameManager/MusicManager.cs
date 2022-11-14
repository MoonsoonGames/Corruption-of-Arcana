using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class MusicManager : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static MusicManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
        }

        public void PlayMusic()
        {

        }
    }
}
