using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacksUtils
{
    [Serializable]
    public class SerializedVector2
    {
        public readonly float X;
        public readonly float Y;
        
        public SerializedVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
