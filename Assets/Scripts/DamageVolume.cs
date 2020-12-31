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
    private bool damagedTaken;

    void Awake()
    {
        nextDamageTimer = Timer;
    }

    void Damage(GameObject gameObject)
    {
        if (!damagedTaken && nextDamageTimer >= Timer)
        {
            CharacterBase character = GetCharacterFromParent(gameObject.transform);
            if (!character)
                return;

            character?.TakeDamage(DamageAmount);
            damagedTaken = true;
            if(ContinuousDamage)
                StartCoroutine(resetDamage());
        }

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

    void OnCollisionStay(Collision collision)
    {
        if (DamageLayers == (DamageLayers | (1 << collision.gameObject.layer)))
        {
            Damage(collision.gameObject);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (DamageLayers == (DamageLayers | (1 << collider.gameObject.layer)))
        {
            Damage(collider.gameObject);
        }
    }

    IEnumerator resetDamage()
    {
        yield return new WaitForFixedUpdate();
        damagedTaken = false;
    }
}
