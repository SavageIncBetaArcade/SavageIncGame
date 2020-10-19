using UnityEngine;

public class ProjectileAbility : AttackAbility
{
    public ProjectileAbility(UseableAbility useableAbility, CharacterBase ownerCharacter) : base(useableAbility, ownerCharacter)
    {

    } 

    public override void Initilise()
    {

    }

    public override void Use()
    {
        ShootProjectile();

        Debug.Log("Shooting Projectile");
    }

    private void ShootProjectile()
    {
        ScriptableProjectileAbility projectileAbility = (ScriptableProjectileAbility) Ability;
        if (projectileAbility.ProjectileGameObject == null)
            return;

        GameObject projectileObject = useableAbility.InstantiateObject(projectileAbility.ProjectileGameObject.gameObject, useableAbility.Origin);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        //Set the projectiles caster ability to this
        if (projectile != null)
        {   
            projectile.SetCastersProjectileAbilty(this);
        }
    }

    public override void Hit(CharacterBase targetCharacter)
    {
        base.Hit(targetCharacter);
    }
}
