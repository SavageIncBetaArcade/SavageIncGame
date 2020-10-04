using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeleeAbility : BaseAbility
{
    private BoxCollider _hitCollider;
    private bool _hasHit;

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

    //Melees attack is done via the animation, if anything enters the trigger
    private void OnTriggerEnter(Collider collider)
    {
        //First check if it has a health component
        //TODO add health component
        if (!_hasHit && OnCooldown())
        {
            Hit();
            _hasHit = true;
        }

    }

    public override void Attack()
    {
        base.Attack();

        _hasHit = false;

        Debug.Log("Attacking");
    }

    public override void Hit()
    {
        base.Hit();

        Debug.Log("Hit");
    }
}
