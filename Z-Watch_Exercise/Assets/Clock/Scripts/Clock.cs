﻿using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform HourHand;
    public Transform MinuteHand;
    public Transform SecondHand;

    void Update()
    {
        var time = TimeProvider.GetTime();

        var minutesSinceMidnight = time.TotalMinutes; // decimal: e.g. 1823.384
        var hoursSinceMidnight = time.TotalHours; // decimal: e.g. 19.290

        // Exercise:
        var rotationSeconds = minutesSinceMidnight % 60;
        var rotationMinutes = hoursSinceMidnight % 60;
        var rotationHours = hoursSinceMidnight % 12 / 12;

        SecondHand.localRotation = Quaternion.Euler(0, (float) rotationSeconds * 360, 0);
        MinuteHand.localRotation = Quaternion.Euler(0, (float) rotationMinutes * 360, 0);
        HourHand.localRotation = Quaternion.Euler(0, (float) rotationHours * 360, 0);
    }
}