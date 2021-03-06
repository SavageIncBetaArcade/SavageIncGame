﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileTrap : MonoBehaviour
{
    public InteractionTrigger[] InteractionTrigger;
    public Projectile Projectile;
    public Transform Origin;
    public AudioSource TriggerAudioSource;
    public LayerMask ProjectileLayerMask;

    private void Awake()
    {
        foreach(var trigger in InteractionTrigger)
        {
            trigger.OnTrigger += fire;
        }
    }

    private void Destroy()
    {
        foreach (var trigger in InteractionTrigger)
        {
            trigger.OnTrigger -= fire;
        }
    }

    private void fire(bool triggered, InteractionTrigger trigger)
    {
        if(!triggered || !Projectile || !Origin)
            return;

        GameObject projectileObject = Instantiate(Projectile.gameObject, Origin.position, Origin.rotation);
        projectileObject.GetComponent<Projectile>().HitLayerMask = ProjectileLayerMask;

        if (TriggerAudioSource && TriggerAudioSource.clip)
            TriggerAudioSource.PlayOneShot(TriggerAudioSource.clip);
    }
}
