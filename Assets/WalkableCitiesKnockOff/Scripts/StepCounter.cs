using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StepCounter
{
    private static int s_currentStepCount;
    private static int s_wholeAmount;
    
    public static string Score => s_currentStepCount.ToString();

    public static void Increment()
    {
        s_currentStepCount++;
    }

    public static void Reset()
    {
        s_currentStepCount = 0;
    }

    public static void Calculate()
    {
        s_wholeAmount += s_currentStepCount;
    }
}
