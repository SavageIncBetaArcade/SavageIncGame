public class EnergyBar : UiBar
{
    private void Start()
    {
        base.Start();
        player.OnMaxEnergyChange += SetMaxValue;
        player.OnReplenishEnergy += SetValue;
        player.OnLoseEnergy += SetValue;
    }

    protected override void SetMaxValue()
    {
        slider.maxValue = player.MaxEnergy;
    }

    protected override void SetValue()
    {
        slider.value = player.CurrentEnergy;
    }
}