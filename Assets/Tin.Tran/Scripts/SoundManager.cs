using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Global;

    public AudioClip TankShoot;
    public AudioClip TankExplore;


    private void Awake()
    {
        Global = this;
    }
    public void Shoot(AudioSource audio)
    {
        audio.clip = TankShoot;
        audio.Play();
    }
    public void Explore(AudioSource audio)
    {
        audio.clip = TankExplore;
        audio.Play();
    }
}
