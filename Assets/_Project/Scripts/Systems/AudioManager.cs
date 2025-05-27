using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [field: SerializeField] public AudioSource DeathSound { get; private set; }
        [field: SerializeField] public AudioSource BoostSound { get; private set; }
        [field: SerializeField] public AudioSource HitSound { get; private set; }
        [field: SerializeField] public AudioSource PauseSound { get; private set; }
        [field: SerializeField] public AudioSource UnpauseSound { get; private set; }
        [field: SerializeField] public AudioSource AsteroidDeathSound { get; private set; }
        [field: SerializeField] public AudioSource AsteroidHitSound { get; private set; }
        [field: SerializeField] public AudioSource ShootSound { get; private set; }
        [field: SerializeField] public AudioSource CritterZapDeathSound { get; private set; }
        [field: SerializeField] public AudioSource CritterBurnDeathSound { get; private set; }

        public void PlaySound(AudioSource sound)
        {
            sound.Stop();
            sound.Play();
        }

        public void PlayModifiedSound(AudioSource sound)
        {
            sound.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
            sound.Stop();
            sound.Play();
        }
    }
}