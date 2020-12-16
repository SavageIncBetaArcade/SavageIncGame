using UnityEngine.UI;

public class HealthBar : Slider
{
    public PlayerBase player;

    private void Start()
    {
        SetMaxValue();
        player.OnMaxHealthChange += SetMaxValue;
        player.OnDamage += SetValue;
        player.OnHeal += SetValue;
    }

    private void SetMaxValue()
    {
        maxValue = player.MaxHealth;
    }

    private void SetValue()
    {
        value = player.CurrentHealth;
    }
}