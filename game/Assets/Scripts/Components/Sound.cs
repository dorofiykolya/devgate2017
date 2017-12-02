using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevGate
{
    [RequireComponent(typeof(AudioSource))]
    public class Sound : MonoBehaviour
    {

        private AudioSource source;

        void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public static void Play(AudioClip clip)
        {
            if (clip == null) return;
            var go = new GameObject("Sound:" + clip.name, typeof(Sound));
            var player = go.GetComponent<Sound>();
            player.source.clip = clip;
            player.source.Play();
            GameObject.Destroy(go, clip.length);
        }

    }
}
