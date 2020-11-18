using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileTrap : MonoBehaviour
{
    public Trigger trigger;
    public Projectile Projectile;
    public Transform Origin;

    private void Awake()
    {
        if (trigger)
        {
            trigger.OnTrigger += fire;
        }
    }

    private void Destroy()
    {
        if (trigger)
        {
            trigger.OnTrigger -= fire;
        }
    }

    private void fire(bool triggered)
    {
        if(!triggered || !Projectile || !Origin)
            return;

        GameObject projectileObject = Instantiate(Projectile.gameObject, Origin.position, Origin.rotation);
    }


}
