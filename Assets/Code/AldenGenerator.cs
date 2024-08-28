using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class AldenGenerator : MonoBehaviour
{
    [SerializeField] private string gasURL;
    private ChatBubble aldenHUD;
    private void Start()
    {
        aldenHUD = gameObject.GetComponentInParent<ChatBubble>();
    }
    public void LogAldenChat(string prompt)
    {
        StartCoroutine(SendDataToGAS(prompt, (response) =>
        {
            Debug.Log("Alden: " + response);
            aldenHUD.UpdateTextBubble(response);
        }));
    }

    private IEnumerator SendDataToGAS(string prompt, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);
        UnityWebRequest www = UnityWebRequest.Post(gasURL, form);

        yield return www.SendWebRequest();
        string response = "";

        if (www.result == UnityWebRequest.Result.Success)
        {
            response = www.downloadHandler.text;
        }

        // Call the callback with the response
        callback?.Invoke(response);
    }
}