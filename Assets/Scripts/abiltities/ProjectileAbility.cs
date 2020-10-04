using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : BaseAbility
{
    public GameObject ProjectileObject;
    public Transform ProjectileSpawnPoint;

    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();

        ShootProjectile();

        Debug.Log("Attacking");
    }

    private void ShootProjectile()
    {
        if(ProjectileObject == null)
            return;

        //If ProjectileSpawnPoint use the current game objects transform (this may cause issues with colliders)
        if (ProjectileSpawnPoint == null)
            ProjectileSpawnPoint = transform;

        GameObject projectileObject = Instantiate(ProjectileObject, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        //Set the projectiles caster ability to this
        if(projectile != null)
            projectile.SetCastersProjectileAbilty(this);
    }
}
