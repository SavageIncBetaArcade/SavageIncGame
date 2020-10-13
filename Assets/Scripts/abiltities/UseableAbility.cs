using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This is the script that allows the character to use abilities
/// The ScriptableUseableAbility that is attached to the ScriptableAbility is the ability that would get called
/// </summary>
public class UseableAbility : MonoBehaviour
{
    //Abilities that use mono behavior are all useable I.E (Weapons, Buffs) - Passive abilities work in a different way
    public ScriptableUseableAbility ScriptableAbility;
    public string AnimationUseBoolName;
    public Animator UseAnimator;

    //TODO pass in the attackers character base
    public delegate void UseAction();
    public event UseAction OnUse;

    //TODO get character base on awake
    public CharacterBase CharacterBase;
    public Transform Origin;

    public List<AbilityModifier> Modifiers;

    private float _lastUseTime;

    private BaseAbility ability;
    private GameObject worldGameObject;

    private ModifierHandler modifierHandler;

    void Initilise()
    {
        ability = AbilityFactory.Create(this, ScriptableAbility, worldGameObject, HitAction);

        //Create a copy of the Modifiers so this ability has its own instance
        modifierHandler = new ModifierHandler(Modifiers);
    }

    protected virtual void Awake()
    {
        if (ScriptableAbility != null && ScriptableAbility.AbilityPrefab != null)
        {
            worldGameObject = Instantiate(ScriptableAbility.AbilityPrefab, transform);
            Initilise();
        }
        else
        {
            Debug.LogWarning("Base ability doesn't have a ability or an ability prefab");
        }

        if (CharacterBase == null)
            Debug.LogWarning("Base ability doesn't have a character base, please make sure to set it");

    }

    protected virtual void Update()
    {
        //TODO check if the current CharacterBase is the player, only attack on left click if player
        if (!OnCooldown() && Input.GetButtonDown("Fire1"))
        {
            ExecuteUse();
            _lastUseTime = Time.time;
        }
    }

    protected bool OnCooldown()
    {
        if (ScriptableAbility.UseAnimationCooldown && UseAnimator != null && !string.IsNullOrWhiteSpace(AnimationUseBoolName))
        {
            return UseAnimator.GetBool(AnimationUseBoolName);
        }

        return !(Time.time >= _lastUseTime + ScriptableAbility.Cooldown);
    }

    private void ExecuteUse()
    {
        modifierHandler.ApplyPreActionModifiers(CharacterBase,null);

        OnUse?.Invoke();

        if (UseAnimator != null)
        {
            UseAnimator.SetBool(AnimationUseBoolName, true);
        }

        ability.Use();

        modifierHandler.ApplyPostActionModifiers(CharacterBase, null);
    }

    public GameObject InstantiateObject(GameObject gameObject, Transform transform)
    {
        return Instantiate(gameObject, transform.position, transform.rotation);
    }

    private void HitAction(CharacterBase attackingcharacter, CharacterBase targetcharacter)
    {
        modifierHandler.ApplyActionModifiers(attackingcharacter, targetcharacter);
    }
}
