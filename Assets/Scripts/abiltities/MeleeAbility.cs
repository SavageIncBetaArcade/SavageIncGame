using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeleeAbility : BaseAbility
{
    public Animation AttackAnimation;
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

    public override void Attack()
    {
        base.Attack();

        Debug.Log("Attacking");
    }
}
