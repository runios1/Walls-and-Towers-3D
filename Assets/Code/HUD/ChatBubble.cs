using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ChatBubble : MonoBehaviour
{
    public TextMeshProUGUI textBubble; // Reference to the Text component
    public GameObject bubblePanel; // Reference to the bubble's Image or Panel

    void Start()
    {
        // Hide the bubble initially
        if (bubblePanel != null)
        {
            bubblePanel.SetActive(false);
        }
    }

    // Method to update the text in the bubble
    public void UpdateTextBubble(string newText)
    {
        if (textBubble != null && bubblePanel != null)
        {
            // Set the text
            textBubble.text = newText;

            // Show the bubble
            bubblePanel.SetActive(true);

            // Calculate the duration to display the text
            float displayDuration = 0.2f * newText.Length;

            // Start a coroutine to hide the bubble after the duration
            StartCoroutine(HideBubbleAfterTime(displayDuration));
        }
    }

    private IEnumerator HideBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Hide the bubble
        if (bubblePanel != null)
        {
            bubblePanel.SetActive(false);
        }
    }
}
