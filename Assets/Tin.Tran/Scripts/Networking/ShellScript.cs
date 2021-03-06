﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    public GameObject Parent;
    public GameObject Trail;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Parent) return;
        if (collision.gameObject.tag == "Tank")
            collision.gameObject.GetComponent<TankBlood>().TakeDamage(Parent);
        Trail.SetActive(false);
        float lifetime = 0;
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        if (systems != null)
        {
            for (int i = 0; i < systems.Length; ++i)
            {
                ParticleSystem system = systems[i];
                system.Play();
                lifetime = Mathf.Max(system.main.duration, lifetime);
            }
        }
        Destroy(gameObject, 0.5f);
    }
}
