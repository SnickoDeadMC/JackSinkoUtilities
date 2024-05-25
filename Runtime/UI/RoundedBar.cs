using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace JacksUtils
{
    public class RoundedBar : MonoBehaviour
    {
        
        [SerializeField] private Image background;
        [SerializeField] private Image backgroundEnd;

        [SerializeField] private Image fill;
        [SerializeField] private Image fillEnd;

        [SerializeField] private float maxWidth = 80;
        [Range(0, 1), SerializeField] private float expandedPercent = 1;
        [Range(0, 1), SerializeField] private float percentageFilled;

        public float MaxWidth => maxWidth;
        public float ExpandedPercent => expandedPercent;

        private void OnValidate()
        {
            UpdateSize(expandedPercent);
            UpdateFill(percentageFilled);
        }

        public void UpdateFill(float percent)
        {
            percentageFilled = percent;

            fill.fillAmount = percent;
            fillEnd.rectTransform.SetPositionX(fill.rectTransform.rect.width * percent);
        }

        public void UpdateSize(float percent)
        {
            expandedPercent = percent;

            background.rectTransform.sizeDelta = new Vector2(maxWidth * percent, background.rectTransform.sizeDelta.y);
            fill.rectTransform.sizeDelta = new Vector2(maxWidth * percent, fill.rectTransform.sizeDelta.y);

            backgroundEnd.rectTransform.SetPositionX(background.rectTransform.rect.width);
            fillEnd.rectTransform.SetPositionX(fill.rectTransform.rect.width * percentageFilled);
        }
    }
}