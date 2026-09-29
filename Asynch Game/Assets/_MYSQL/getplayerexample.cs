// GetPlayersExample.cs

using UnityEngine;

public class GetPlayersExample : MonoBehaviour
{
    [SerializeField] private GetPlayersResponse getPlayersResponse;
    private PlayerDisplayWindow PlayerDisplayWindow;

    private async void Start()
    {
        PlayerDisplayWindow = GetComponent<PlayerDisplayWindow>();
        getPlayersResponse = await GenericApiClient.Instance.GetPlayers();
        //PlayerDisplayWindow.Refresh(getPlayersResponse);
    }
}