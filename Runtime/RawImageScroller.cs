using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace JacksUtils
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageScroller : MonoBehaviour
    {

        [SerializeField] private Vector2 speed;

        private RawImage rawImage => GetComponent<RawImage>();
        
        private void Update()
        {
            Rect uvRect = rawImage.uvRect;
            uvRect.x += speed.x * Time.deltaTime;
            uvRect.y += speed.y * Time.deltaTime;
            rawImage.uvRect = uvRect;
        }
        
    }
}
