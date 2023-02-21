using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class TooltipBox : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        public bool followMouse = false;
        RectTransform bg;
        public RectTransform canvas;
        public float xMultiplier = 1.87f;
        public float yMultiplier = 2.475f;

        private void Start()
        {
            bg = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (followMouse)
            {
                //https://www.youtube.com/watch?v=dzkFdkwzVhs

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = mousePosition;

                Vector2 tooltipPos = transform.GetComponent<RectTransform>().anchoredPosition;
                float width = bg.rect.width * xMultiplier;
                float height = bg.rect.height * yMultiplier;

                if (tooltipPos.x + width > canvas.rect.width)
                {
                    tooltipPos.x = canvas.rect.width - width;
                }
                else if (Mathf.Abs(tooltipPos.x) + width > canvas.rect.width)
                {
                    tooltipPos.x = -canvas.rect.width + width;
                }

                if (tooltipPos.y + height > canvas.rect.height)
                {
                    tooltipPos.y = canvas.rect.height - height;
                }
                else if (Mathf.Abs(tooltipPos.y) + height > canvas.rect.height)
                {
                    tooltipPos.y = -canvas.rect.width + height;
                }

                transform.GetComponent<RectTransform>().anchoredPosition = tooltipPos;
            }
        }

        public void SetText(string titleText, string descText)
        {
            if (IconManager.instance == null)
            {
                title.text = titleText;
                description.text = descText;
                return;
            }

            if (title != null)
            {
                title.text = IconManager.instance.ReplaceText(titleText);
            }

            if (description != null)
            {
                description.text = IconManager.instance.ReplaceText(descText);
            }
        }
    }
}
