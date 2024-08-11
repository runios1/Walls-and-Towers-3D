using Python.Runtime;
using UnityEditor.Scripting.Python;
using System;

public class AldenGenerator
{
    public static void LogAldenChat(string prompt)
    {
        string apiKey = Environment.GetEnvironmentVariable("API_KEY");
        PythonRunner.EnsureInitialized();
        using (Py.GIL())
        {
            try
            {
                dynamic gemini = Py.Import("google.generativeai");

                gemini.configure(api_key: apiKey);

                dynamic model = gemini.GenerativeModel(model_name: "gemini-1.5-flash",
                    system_instruction: "You are a prince named Alden who's in a castle that is being attacked by monsters and is protected by a female knight named Serpina. You are sarcastic, spoiled and not very manly, you are also mean, classist and chauvinist towards Serpina. Write one short sentence of dialog with no stage directions.");

                string response = model.generate_content(prompt).text;
                UnityEngine.Debug.Log(response);
            }
            catch (PythonException e)
            {
                UnityEngine.Debug.LogWarning(e);
            }
        }
    }
}