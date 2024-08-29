using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SeraphinaChat : MonoBehaviour
{
    public TextMeshProUGUI textBubble; // Reference to the Text component
    public GameObject bubblePanel; // Reference to the bubble's Image or Panel
    public AldenGenerator aldenGenerator;
    public bool victory;
    private bool doneFetching;
    // Start is called before the first frame update
    void Start()
    {
        bubblePanel.SetActive(true);
        textBubble.SetText("Loading...");
        if(victory)
            aldenGenerator.VictoryMessage((message) => UpdateTextBubble(message));
        else
            aldenGenerator.DefeatMessage((message) => UpdateTextBubble(message));
    }
    
    public void UpdateTextBubble(string newText)
    {
        if (textBubble != null && bubblePanel != null)
        {
            // Set the text
            textBubble.text = newText;
            textBubble.ForceMeshUpdate();
        }
    }
    // Update is called once per frame

}
