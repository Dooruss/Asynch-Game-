// GetPlayersExample.cs

using UnityEngine;

public class GetPlayersExample : MonoBehaviour
{
    [SerializeField] private GetPlayersResponse getPlayersResponse;

    private async void Start()
    {
        getPlayersResponse = await GenericApiClient.Instance.GetPlayers();
    }
}