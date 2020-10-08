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
    private Rigidbody _rigidbody;

    //The ProjectileAbility that was used to cast this project (null of none)
    private ProjectileAbility _castersProjectileAbility;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = gravityAffected;
    }

    void Start()
    {
        startPosition = transform.position;
        _rigidbody.AddForce(initialForce * transform.forward, ForceMode.Impulse);
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

        Impact();
    }

    void Impact()
    {
        //First check if it has a health component
        Debug.Log("Projectile Hit");

        Destroy(gameObject);
    }

    public void SetCastersProjectileAbilty(ProjectileAbility projectileAbility)
    {
        _castersProjectileAbility = projectileAbility;

        ScriptableProjectileAbility scriptableProjectile = (ScriptableProjectileAbility)projectileAbility.Ability;
        damage = scriptableProjectile.Damage;
        initialForce = scriptableProjectile.InitialForce;
        gravityAffected = scriptableProjectile.GravityAffected;
        _rigidbody.useGravity = gravityAffected;
        RangeCutoff = scriptableProjectile.Range;
    }
}
