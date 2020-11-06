using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float initialForce;
    [SerializeField]
    private bool gravityAffected = true;
    [SerializeField]
    private float lifeSpan = 2.5f;
    [SerializeField]
    private float RangeCutoff = 100.0f;

    private Vector3 startPosition;
    private Rigidbody projectileRigidbody;

    //The ProjectileAbility that was used to cast this project (null of none)
    private ProjectileAbility castersProjectileAbility;

    void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        projectileRigidbody.useGravity = gravityAffected;
    }

    void Start()
    {
        startPosition = transform.position;
        projectileRigidbody.AddForce(initialForce * transform.forward, ForceMode.Impulse);
        Destroy(gameObject, lifeSpan);
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > RangeCutoff)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //First check if it has a health component
        //TODO add health component and pass into hit

        Impact(collision.gameObject, collision.GetContact(0).point, collision.GetContact(0).normal);
    }

    void Impact(GameObject hitGameObject, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (castersProjectileAbility != null)
        {
            //First check if it has a health component
            Debug.Log("Projectile Hit");

            ScriptableProjectileAbility projectileAbility =
                castersProjectileAbility.Ability as ScriptableProjectileAbility;

            if (projectileAbility == null)
                Debug.LogError("Projectile: ScriptableAbility is not of type ScriptableProjectileAbility");

            castersProjectileAbility.Hit(hitGameObject, projectileAbility != null ? projectileAbility.Damage : 0.0f,
                hitPoint, transform.forward, hitNormal, projectileAbility.AddOwnerBaseAttack);
        }

        Destroy(gameObject);
    }

    public void SetCastersProjectileAbilty(ProjectileAbility projectileAbility)
    {
        castersProjectileAbility = projectileAbility;

        ScriptableProjectileAbility scriptableProjectile = (ScriptableProjectileAbility)projectileAbility.Ability;
        damage = scriptableProjectile.Damage;
        initialForce = scriptableProjectile.InitialForce;
        gravityAffected = scriptableProjectile.GravityAffected;
        projectileRigidbody.useGravity = gravityAffected;
        RangeCutoff = scriptableProjectile.Range;
    }

    
}
