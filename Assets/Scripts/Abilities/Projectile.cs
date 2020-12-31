using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PortalableObject))]
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

    [SerializeField]
    private AudioClip ImpactSound;
    [SerializeField]
    private AudioSource ImpactAudioSource;

    [SerializeField] 
    private LayerMask HitLayerMask;

    private Vector3 startPosition;
    private Rigidbody projectileRigidbody;
    private PortalableObject portalableObject;

    //The ProjectileAbility that was used to cast this project (null of none)
    private ProjectileAbility castersProjectileAbility;

    void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        projectileRigidbody.useGravity = gravityAffected;
        portalableObject = GetComponent<PortalableObject>();
        portalableObject.HasTeleported += PortalableObjectOnHasTeleported;
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
        if ((HitLayerMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            Impact(collision.gameObject, collision.GetContact(0).point, collision.GetContact(0).normal);
        }
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
                hitPoint, transform.forward, hitNormal);
        }
        else
        {
            CharacterBase hitCharacter = AttackAbility.GetParentCharacterBase(hitGameObject.transform);
            if (hitCharacter)
            {
                hitCharacter.TakeDamage(damage);
            }

            if (ImpactSound && ImpactAudioSource)
            {
                ImpactAudioSource.PlayOneShot(ImpactSound);
            }
        }

        if (ImpactSound)
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<SphereCollider>().enabled = false;
            this.enabled = false;
            for (int i = 0; i < transform.childCount-1; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            Destroy(gameObject, ImpactSound.length);
        }
        else
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

    public virtual void PortalableObjectOnHasTeleported(Portal startPortal, Portal endPortal, Vector3 newposition, Quaternion newrotation)
    {
        startPosition = newposition;
        projectileRigidbody.velocity = Vector3.zero;
        projectileRigidbody.AddForce(initialForce * transform.forward, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        portalableObject.HasTeleported -= PortalableObjectOnHasTeleported;
    }
}
