using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome
{
    private float stopwatch;
    private float interval;

    public Metronome(float interval) {
        this.interval = interval;
    }

    // add time to stopwatch in seconds
    public void AddTimeToStopwatch(float deltaTime) {
        stopwatch += deltaTime;
    }

    public bool Triggered() {

        if (stopwatch >= interval) {
            ResetMetronome();
            return true;
        }
        return false;
    }
    private void ResetMetronome() {
        stopwatch = 0.0f;
    }
}
