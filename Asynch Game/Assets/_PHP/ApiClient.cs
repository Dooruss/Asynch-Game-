using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private const string ApiUrl = "http://localhost/example.php";

    [SerializeField] private RandomTitleResponse response;

    private async void Start()
    {
        await RandomTitle("Henry");
    }

    private async Awaitable RandomTitle(string name)
    {
        RandomTitleRequest requestData = new RandomTitleRequest
        {
            action = "random_title",
            name = name
        };

        // Zet het C# object om naar JSON.
        string json = JsonUtility.ToJson(requestData);

        // Maak een POST request naar ons PHP script.
        using UnityWebRequest request =
            new UnityWebRequest(ApiUrl, UnityWebRequest.kHttpVerbPOST);

        // Zet de JSON-string om naar bytes.
        byte[] body = Encoding.UTF8.GetBytes(json);

        // Voeg de JSON toe aan de body van het request.
        request.uploadHandler = new UploadHandlerRaw(body);

        // Bereid Unity voor om een response te ontvangen.
        request.downloadHandler = new DownloadHandlerBuffer();

        // Vertel de server dat we JSON versturen.
        request.SetRequestHeader("Content-Type", "application/json");

        // Verstuur het request en wacht op de response.
        await request.SendWebRequest();

        // Haal de response van de server op.
        string responseJson = request.downloadHandler.text;

        // Zet de JSON-response om naar een C# object.
        response = JsonUtility.FromJson<RandomTitleResponse>(responseJson);
    }
}

[Serializable]
public class RandomTitleRequest
{
    public string action;
    public string name;
}

[Serializable]
public class RandomTitleResponse
{
    public bool success;
    public string name;
    public string message;
}