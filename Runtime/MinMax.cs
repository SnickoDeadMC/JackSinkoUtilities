using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public struct MinMax
{

    public float Min;
    public float Max;

    public float Difference => Max - Min;
    
    public MinMax(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public float GetRandomValue()
    {
        return Random.Range(Min, Max);
    }
    
}
