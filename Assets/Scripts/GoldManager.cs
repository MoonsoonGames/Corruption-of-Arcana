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
    public class GoldManager : MonoBehaviour
    {

        #region Singleton
        //Code from last year

        public static GoldManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                //DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                return;
            }
        }

        #endregion

        public int goldCount;

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(goldCount);
        }

        public void AddGold(int amtToAdd)
        {
            goldCount += amtToAdd;
        }
    }
}
