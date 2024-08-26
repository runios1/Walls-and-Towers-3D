using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int totalWaveCount;
    private int curWave;

    public void ResetCounter(int totalWaveCount)
    {
        this.totalWaveCount = totalWaveCount;
        curWave = 0;
        text.text = curWave + "|" + totalWaveCount;
    }

    public void IncreaseCounter()
    {
        curWave++;
        text.text = curWave + "|" + totalWaveCount;
    }
}
