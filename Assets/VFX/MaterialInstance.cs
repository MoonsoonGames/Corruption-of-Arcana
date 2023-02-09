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
    public class MaterialInstance : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        public float dissolveAmount;
        public Material mat;
        
        // Start is called before the first frame update
        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
        }

        // Update is called once per frame
        void Update()
        {
            mat.SetFloat("_Disolve_amount", dissolveAmount);
        }

        public void SetColour(Color color)
        {
            mat.SetVector("_Color", color);
        }
    }
}
