using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float InitialForce;
    public bool GravityAffected = true;
    public float LifeSpan = 2.5f;

    private Rigidbody _rigidbody;

    //The ProjectileAbility that was used to cast this project (null of none)
    private ProjectileAbility _castersProjectileAbility;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = GravityAffected;
    }

    void Start()
    {
        _rigidbody.AddForce(InitialForce * transform.forward, ForceMode.Impulse);
        Destroy(gameObject, LifeSpan);
    }

    void OnCollisionEnter(Collision collision)
    {
        //First check if it has a health component
        //TODO add health component and pass into hit

        Impact();
    }

    void Impact()
    {
        if (_castersProjectileAbility != null)
            _castersProjectileAbility.Hit();

        Destroy(gameObject);
    }

    public void SetCastersProjectileAbilty(ProjectileAbility projectileAbility)
    {
        _castersProjectileAbility = projectileAbility;
    }
}
