using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingHealthBar : HealthBar
{
    public Gradient gradient;
    public Image fill;

    public override void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public override void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
