using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SpawnArcanaSymbol : MonoBehaviour
    {
        public Object[] arcanaSymbols;

        public void SpawnArcanaSymbols(int count)
        {
            if (count < arcanaSymbols.Length)
                Instantiate(arcanaSymbols[count], this.transform);
        }
    }
}
