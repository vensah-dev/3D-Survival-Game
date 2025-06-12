using System;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    public TextMeshProUGUI FPSDisplay;

    public float UpdateFrequency = 1f;
    public int RoundTo;
    float time;
    int FrameCount;

    void Update()
    {
        time += Time.deltaTime;

        FrameCount++;

        if(time >= UpdateFrequency)
        {
            double FPS = Math.Round(FrameCount / time, RoundTo);
            FPSDisplay.text = "FPS:" + FPS.ToString();

            time -= UpdateFrequency;
            FrameCount = 0;
        }
    }
}
