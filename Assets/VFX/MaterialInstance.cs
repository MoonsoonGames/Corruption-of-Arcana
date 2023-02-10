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
        Material mat;

        // Start is called before the first frame update
        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mat = spriteRenderer.material;
            
            SetColour(new Color(1, 1, 1, 255));
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

        public void SetDissolveColor(Color color)
        {
            mat.SetVector("_Disolve_Colour", color);
        }

        public void SetDissolve(float dissolveAmount)
        {
            this.dissolveAmount = dissolveAmount;
            mat.SetFloat("_Disolve_amount", dissolveAmount);
        }

        [ContextMenu("Blue")]
        public void SetColourBlue()
        {
            SetColour(new Color(0.298104316f, 0.298104316f, 0.726415098f, 255));
        }

        [ContextMenu("normal")]
        public void SetColourNormal()
        {
            SetColour(new Color(1, 1, 1, 255));
        }
    }
}
