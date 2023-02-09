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
    public class MeshMaterialInstance : MonoBehaviour
    {
        MeshRenderer meshRenderer;
        public float dissolveAmount;
        public Material mat;
        
        // Start is called before the first frame update
        public void Setup(Texture texture)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            mat = meshRenderer.material;
            mat.SetTexture("_Sprite", texture);
        }

        // Update is called once per frame
        void Update()
        {
            mat.SetFloat("_Disolve_amount", dissolveAmount);
        }
    }
}
