using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public float DamageAmount;
    public bool ContinuousDamage;
    public float Timer;
    public LayerMask DamageLayers;

    private float nextDamageTimer;

    void Awake()
    {
        nextDamageTimer = Timer;
    }

    void Damage(GameObject gameObject)
    {
        if (nextDamageTimer >= Timer)
        {
            CharacterBase character = GetCharacterFromParent(gameObject.transform);
            if (!character)
                return;

            character?.TakeDamage(DamageAmount);
            nextDamageTimer = 0.0f;
        }
    }

    void Update()
    {
        nextDamageTimer += Time.deltaTime;
    }

    private CharacterBase GetCharacterFromParent(Transform currentTransform)
    {
        CharacterBase character = currentTransform.GetComponent<CharacterBase>();

        if (character)
            return character;

        if (!currentTransform.parent)
            return null;

        return GetCharacterFromParent(currentTransform.parent);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!ContinuousDamage && (DamageLayers == (DamageLayers | (1 << collision.gameObject.layer))))
        {
            Damage(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!ContinuousDamage && (DamageLayers == (DamageLayers | (1 << collider.gameObject.layer))))
        {
            Damage(collider.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (ContinuousDamage && (DamageLayers == (DamageLayers | (1 << collision.gameObject.layer))))
        {
            Damage(collision.gameObject);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (ContinuousDamage && (DamageLayers == (DamageLayers | (1 << collider.gameObject.layer))))
        {
            Damage(collider.gameObject);
        }
    }
}
