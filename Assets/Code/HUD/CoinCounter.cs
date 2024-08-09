using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        ResetCounter();
    }

    public void ResetCounter()
    {
        count = 0;
        text.text = "0";
    }

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
