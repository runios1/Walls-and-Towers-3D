using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CoinsChangeController coinsChangeController;
    private int count;

    public void IncreaseCounter(int amount)
    {
        count += amount;
        text.text = "" + count;
        coinsChangeController.ChangeCoins(amount, true);
    }

    public void DecreaseCounter(int amount)
    {
        count -= amount;
        text.text = "" + count;
        coinsChangeController.ChangeCoins(amount, false);
    }
}
