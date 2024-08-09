using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int count;

    public void IncreaseCounter(int amount)
    {
        count += amount;
        text.text = "" + count;
    }

    public void DecreaseCounter(int amount)
    {
        count -= amount;
        text.text = "" + count;
    }
}
