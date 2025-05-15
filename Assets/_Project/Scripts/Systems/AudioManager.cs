using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [field: SerializeField] public AudioSource DeathSound { get; private set; }
    [field: SerializeField] public AudioSource Fire { get; private set; }
    [field: SerializeField] public AudioSource Hit { get; private set; }
    [field: SerializeField] public AudioSource Pause { get; private set; }
    [field: SerializeField] public AudioSource Unpause { get; private set; }

    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }
}