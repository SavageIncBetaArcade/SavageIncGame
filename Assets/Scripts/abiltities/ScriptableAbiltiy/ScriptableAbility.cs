using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableAbility : ScriptableObject
{
    public string AbilityName;
    public Sprite AbilitySprite;
    public float Cooldown;
    public bool UseAnimationCooldown;
    public GameObject AbilityPrefab;

    public abstract void Initialize(GameObject hand);
    public abstract void UseAbility();
}
