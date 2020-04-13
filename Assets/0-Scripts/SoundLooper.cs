using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLooper : MonoBehaviour {
    public AudioSource targetAudioSource;
    public float startTime;
    public float endTime;
    public int sampleRate;
    public bool byPass;

    private void OnEnable() {
        PlaySound();
    }
    
    public void PlaySound() {
        targetAudioSource.timeSamples = (int)(startTime * sampleRate);
        targetAudioSource.Play();
        byPass=false;
    }

    public void StopSound() {
        targetAudioSource.Stop();
    }

    void Update() {
        if (!byPass) {
            if (targetAudioSource.timeSamples>(int)(endTime * sampleRate)) {
                targetAudioSource.timeSamples = (int)(startTime * sampleRate);
            }
        }
    }


}
