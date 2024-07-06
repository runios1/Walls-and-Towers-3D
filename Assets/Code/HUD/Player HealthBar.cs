using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    public override void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public override void SetHealth(float health)
    {
        slider.value = health;
    }
}
