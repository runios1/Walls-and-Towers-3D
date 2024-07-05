using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{

	public Slider slider;

	public abstract void SetMaxHealth(float health);

	public abstract void SetHealth(float health);

}
