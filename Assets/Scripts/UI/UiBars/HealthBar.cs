public class HealthBar : UiBar
{
    private void Start()
    {
        base.Start();
        player.OnMaxHealthChange += SetMaxValue;
        player.OnDamage += SetValue;
        player.OnHeal += SetValue;
    }

    protected override void SetMaxValue()
    {
        slider.maxValue = player.MaxHealth;
    }

    protected override void SetValue()
    {
        slider.value = player.CurrentHealth;
    }
}