using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeleeAbility : BaseAbility
{
    private BoxCollider _hitCollider;

    protected override void Awake()
    {
        base.Awake();

        _hitCollider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    private void OnTriggerEnter(Collider collider)
    {
        //First check if it has a health component
        //TODO add health component
        Hit();

    }

    public override void Attack()
    {
        base.Attack();

        Debug.Log("Attacking");
    }

    protected override void Hit()
    {
        base.Hit();

        Debug.Log("Hit");
    }
}
