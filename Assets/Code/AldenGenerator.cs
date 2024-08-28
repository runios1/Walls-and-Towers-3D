using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class AldenGenerator : MonoBehaviour
{
    [SerializeField] private string gasURL;
    [SerializeField] public string prompt;
    void Start()
    {
        StartCoroutine(SendDataToGAS());
    }

    public IEnumerator SendDataToGAS()
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

        Debug.Log("Alden: " + response);
    }
}