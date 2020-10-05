using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    private int attackModifier, defenseModifier, health, energy, mana;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Getters
    public int GetAttackModifier()
    {
        return attackModifier;
    }

    public int GetDefenseModifier()
    {
        return defenseModifier;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public int GetMana()
    {
        return mana;
    }

    //Setters
    public void SetAttackModifier(int attackMod)
    {
        attackModifier = attackMod;
    }

    public void SetDefenseModifier(int defenseMod)
    {
        defenseModifier = defenseMod;
    }

    public void SetHealth(int healthAmt)
    {
        health = healthAmt;
    }

    public void SetEnergy(int energyAmt)
    {
        energy = energyAmt;
    }

    public void SetMana(int manaAmt)
    {
        mana = manaAmt;
    }
}
