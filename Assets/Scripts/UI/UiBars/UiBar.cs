using UnityEngine;
using UnityEngine.UI;

public abstract class UiBar : MonoBehaviour
{
    public PlayerBase player;
    protected Slider slider;
    
    protected void Start()
    {
        slider = GetComponent<Slider>();
        SetMaxValue();
        SetValue();
    }

    protected abstract void SetMaxValue();
    protected abstract void SetValue();
}