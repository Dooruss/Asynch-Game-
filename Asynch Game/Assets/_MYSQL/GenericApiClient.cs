// GenericApiClient.cs

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GenericApiClient
{
    private const string ApiUrl = "http://localhost/fantasy_players.php";

    private static GenericApiClient instance;

    public static GenericApiClient Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GenericApiClient();
            }
            return instance;
        }
    }

    // Elke soort request krijgt zijn eigen method, maar allemaal gebruiken ze
    // Dezelfde private SendRequest method hierbeneden
    public async Awaitable<GetPlayersResponse> GetPlayers()
    {
        GetPlayersRequest request = new GetPlayersRequest
        {
            action = "get_players"
        };

        return await SendRequest<GetPlayersResponse>(request);
    }

    // Update an existing player's score on the server
    public async Awaitable<UpdatePlayerResponse> UpdatePlayer(int id, int score)
    {
        UpdatePlayerRequest request = new UpdatePlayerRequest
        {
            action = "update_player",
            id = id,
            score = score
        };

        return await SendRequest<UpdatePlayerResponse>(request);
    }

    // De generieke SendRequest method
    private async Awaitable<TResponse> SendRequest<TResponse>(object requestData)
        where TResponse : ResponseBase
    {

        string json = JsonUtility.ToJson(requestData);

        using UnityWebRequest request =
            new UnityWebRequest(ApiUrl, UnityWebRequest.kHttpVerbPOST);

        byte[] body = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();

        string responseJson = request.downloadHandler.text;

        if (string.IsNullOrEmpty(responseJson))
        {
            Debug.LogError(
                $"API returned no JSON.\n" +
                $"HTTP Status: {request.responseCode}\n" +
                $"Error: {request.error}"
            );

            return null;
        }

        try
        {
            TResponse response = JsonUtility.FromJson<TResponse>(responseJson);

            if (response == null)
            {
                Debug.LogError(
                    $"Could not deserialize API response:\n{responseJson}"
                );

                return null;
            }

            return response;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Failed to deserialize API response.\n" +
                $"Response:\n{responseJson}\n\n" +
                $"Exception:\n{exception}"
            );

            return null;
        }
    }
}

// Requests.cs
[System.Serializable]
public abstract class RequestBase
{
    public string action;
}

[System.Serializable]
public class GetPlayersRequest : RequestBase
{

}

[System.Serializable]
public class CreatePlayerRequest : RequestBase
{
    public string username;
    public int score;
    public string favoriteUnit;
}

[System.Serializable]
public class UpdatePlayerRequest : RequestBase
{
    public int id;
    public int score;
}

[System.Serializable]
public class DeletePlayerRequest : RequestBase
{
    public int id;
}

// Responses.cs

[System.Serializable]
public abstract class ResponseBase
{
    public bool success;
    public string message;
    public string error;
}

[System.Serializable]
public class GetPlayersResponse : ResponseBase
{
    public List<PlayerEntry> entries;
}

[System.Serializable]
public class CreatePlayerResponse : ResponseBase
{

}

[System.Serializable]
public class UpdatePlayerResponse : ResponseBase
{

}

[System.Serializable]
public class DeletePlayerResponse : ResponseBase
{

}

// PlayerEntry.cs

[System.Serializable]
public class PlayerEntry
{
    public int id;
    public string username;
    public int score;
    public string favoriteUnit;
}