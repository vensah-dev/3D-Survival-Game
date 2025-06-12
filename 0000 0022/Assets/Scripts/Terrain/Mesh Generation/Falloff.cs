using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Falloff
{
    public static float[,] GenerateFalloffMap(int Size, float A, float B)
    {

        float[,] falloffMap = new float[Size, Size];
        for (int i = 0; i < Size; i++)
        {
            for (int J = 0; J < Size; J++)
            {
                float x = i / (float)Size * 2 - 1;
                float y = J / (float)Size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                float a = A;
                float b = B;

                value = Mathf.Pow(value, a) / ((Mathf.Pow(value, a) + Mathf.Pow(b - (b * value), a)));

                falloffMap[i, J] = value;
            }
        }

        return falloffMap;
    }

}
