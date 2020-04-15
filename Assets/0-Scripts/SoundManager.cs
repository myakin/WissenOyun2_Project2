using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    public AudioSource ambience;
    public AudioSource wings;
    public AudioSource hit;
    public AudioSource descent;
    private bool hasBeganPlayingSounds;

    private void Awake() {
        if (SoundManager.Instance==null) {
            SoundManager.Instance = this;
        } else {
            if (SoundManager.Instance!=this) {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundsOnFail() {
        if(!hasBeganPlayingSounds) {
            hasBeganPlayingSounds = true;
            wings.Stop();

            float descentPitchValue = Random.Range(0.5f, 2f);
            descent.pitch = descentPitchValue;
            descent.Play();
        }
        // if (!hit.isPlaying) {
            float pitchValue = Random.Range(0.7f, 4f);
            hit.pitch = pitchValue;
            hit.Play();
        // }
    }

    public void ResetSounds() {
        hasBeganPlayingSounds = false;
        wings.Play();
        hit.Stop();
        descent.Stop();
    }

}
