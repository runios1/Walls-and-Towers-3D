using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AldenHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private SpriteRenderer textBubble;

    private Vector2 padding = new(4f, 2f);


    public void setText(string newText)
    {
        text.SetText(newText);
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);

        textBubble.size = textSize + padding;
        textBubble.transform.position = text.transform.position + new Vector3(0, textSize.y / 2, 0);
    }
}
