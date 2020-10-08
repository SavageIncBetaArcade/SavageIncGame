using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PassiveStatAbility")]
public class ScriptablePassiveStatAbility : ScriptablePassiveAbility
{
    public StatType StatType;
    public float Modifier;

    public override void Apply(CharacterBase characterBase)
    {
        characterBase.ApplyStatModifier(StatType, Modifier);
    }
}
