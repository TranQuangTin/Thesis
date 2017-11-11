using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Alta.Tools
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Global;
        private Dictionary<string, AudioSource> sounds;
        void Awake()
        {
            if (Global == null)
            {
                Global = this;
            }
            sounds = new Dictionary<string, AudioSource>();
        }
        // Use this for initialization
        void Start()
        {
            AudioSource[] sounds = GetComponentsInChildren<AudioSource>();
            if (sounds != null)
            {
                foreach (AudioSource a in sounds)
                {
                    if (!this.sounds.ContainsKey(a.name))
                    {
                        this.sounds.Add(a.name, a);
                    }
                }
            }
        }

        public void Play(string name,bool loop= false, Func<AudioSource,bool>f=null)
        {
            
            if (this.sounds.ContainsKey(name))
            {
                if (f == null || f(this.sounds[name]))
                {
                    this.sounds[name].loop = loop;
                    this.sounds[name].Play();
                }
            }
        }

        public void Stop(string name, Func<AudioSource,bool> f=null)
        {
            if (this.sounds.ContainsKey(name))
            {
                if (f == null || f(this.sounds[name]))
                    this.sounds[name].Stop();
            }
        }
    }
}
