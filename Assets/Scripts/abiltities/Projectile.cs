using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float InitialForce;
    public bool GravityAffected = true;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCastersProjectileAbilty(ProjectileAbility projectileAbility)
    {
        _castersProjectileAbility = projectileAbility;
    }
}
