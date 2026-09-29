using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDisplayWindow : MonoBehaviour
{
    public playerData RoyalData;
    // Een UI Document voeg je toe als een VisualTreeAsset in de inspector.
    [SerializeField] private VisualTreeAsset rowAsset;
    [TextArea(4, 10)] public string json;
    [SerializeField] private GetPlayersResponse getPlayersResponse;

    // UI-elementen
    private PanelRenderer panelRenderer;
    private ScrollView scrollView;
    private Button refreshButton;
    private Button insertButton;
    private Button updateButton;
    private Button deleteButton;
    //insert
    private TextField InsertUsernameField;
    private TextField InsertScoreField;
    private TextField InsertFavoriteUnitField;
    //update
    private TextField UpdateIDField;
    private TextField UpdateScoreField;
    //delete
    private TextField DeleteIDField;

    private void Start()
    {
        RoyalData = JsonUtility.FromJson<playerData>(json);
    }
    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        UnregisterCallbacks();
    }

    private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement, int version)
    {
        scrollView = rootElement.Q<ScrollView>("PlayerListScrollView");
        refreshButton = rootElement.Q<Button>("RefreshButton");
        insertButton = rootElement.Q<Button>("InsertButton");
        updateButton = rootElement.Q<Button>("UpdateButton");
        deleteButton = rootElement.Q<Button>("DeleteButton");
        //textfields
        InsertUsernameField = rootElement.Q<TextField>("InsertUsernameField");
        InsertScoreField = rootElement.Q<TextField>("InsertScoreField");
        InsertFavoriteUnitField = rootElement.Q<TextField>("InsertFavoriteUnitField");
        UpdateIDField = rootElement.Q<TextField>("UpdateIDField");
        UpdateScoreField = rootElement.Q<TextField>("UpdateScoreField");
        DeleteIDField = rootElement.Q<TextField>("DeleteIDField");

        UnregisterCallbacks();
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        refreshButton.clicked += OnRefreshClicked;
        insertButton.clicked += OnInsertClicked;
        updateButton.clicked += OnUpdateClicked;
        deleteButton.clicked += OnDeleteClicked;
    }

    private void UnregisterCallbacks()
    {
        if (refreshButton != null) { refreshButton.clicked -= OnRefreshClicked; }
        if (insertButton != null) { insertButton.clicked -= OnInsertClicked; }
        if (updateButton != null) { updateButton.clicked -= OnUpdateClicked; }
        if (deleteButton != null) { deleteButton.clicked -= OnDeleteClicked; }
    }

    public void Refresh(GetPlayersResponse response)
    {

        for (int i = 0; i < response.entries.Count; i++)
        {
            // Maak een nieuwe rij aan door de VisualTreeAsset te klonen
            PlayerEntry DataNumber = response.entries[i];
            VisualElement row = rowAsset.CloneTree();
            row.Q<Label>("FirstLabel").text = DataNumber.id.ToString();
            row.Q<Label>("SecondLabel").text = DataNumber.username;
            row.Q<Label>("ThirdLabel").text = DataNumber.score.ToString();
            row.Q<Label>("FourthLabel").text = DataNumber.favoriteUnit;

            // Voeg de rij toe aan de ScrollView
            scrollView.Add(row);
           
        }
    }
    private async void OnRefreshClicked()
    {
        getPlayersResponse = await GenericApiClient.Instance.GetPlayers();
        scrollView?.Clear();
        if (getPlayersResponse != null)
        {
            Refresh(getPlayersResponse);
        }
    }

    private void OnInsertClicked()
    {
        // Lees invoer uit de velden
        string username = InsertUsernameField?.value?.Trim();
        string scoreText = InsertScoreField?.value?.Trim();
        string favoriteUnit = InsertFavoriteUnitField?.value?.Trim();

        //reset invoervelden
        InsertUsernameField.value = string.Empty;
        InsertScoreField.value = string.Empty;
        InsertFavoriteUnitField.value = string.Empty;

        _ = InsertPlayerAndRefreshAsync(username, scoreText , favoriteUnit);

        InsertUsernameField.value = string.Empty;
        InsertScoreField.value = string.Empty;
        InsertFavoriteUnitField.value = string.Empty;
        OnRefreshClicked();
    }

    private void OnUpdateClicked()
    {
        // Voeg hier de logica toe om een speler bij te werken
        string idText = UpdateIDField?.value?.Trim();
        string scoreText = UpdateScoreField?.value?.Trim();

        if (!int.TryParse(idText, out int id))
        {
            Debug.LogError("Invalid ID for update.");
            return;
        }

        if (!int.TryParse(scoreText, out int newScore))
        {
            Debug.LogError("Invalid score for update.");
            return;
        }

        // Call API to update the player on the server, then refresh from server
        UpdateScoreField.value = string.Empty;
        UpdateIDField.value = string.Empty;

        _ = UpdatePlayerAndRefreshAsync(id, newScore);


        UpdateIDField.value = string.Empty;
        UpdateScoreField.value = string.Empty;
        OnRefreshClicked();

    }

    private async Awaitable UpdatePlayerAndRefreshAsync(int id, int newScore)
    {
        var updateResponse = await GenericApiClient.Instance.UpdatePlayer(id, newScore);

        if (updateResponse == null)
        {
            Debug.LogError("Update request failed or returned no response.");
            return;
        }

        if (!updateResponse.success)
        {
            Debug.LogError($"Update failed: {updateResponse.message} {updateResponse.error}");
            return;
        }

        // After successful update, fetch latest players and refresh UI
        getPlayersResponse = await GenericApiClient.Instance.GetPlayers();
        scrollView?.Clear();
        if (getPlayersResponse != null)
        {
            Refresh(getPlayersResponse);
        }
    }

    private async Awaitable InsertPlayerAndRefreshAsync(string username, string scoreText, string favoriteUnit)
    {
        if (!int.TryParse(scoreText, out int score))
        {
            Debug.LogError("Invalid score for insert.");
            return;
        }
        var insertResponse = await GenericApiClient.Instance.CreatePlayer(username, score, favoriteUnit);
        if (insertResponse == null)
        {
            Debug.LogError("Insert request failed or returned no response.");
            return;
        }
        if (!insertResponse.success)
        {
            Debug.LogError($"Insert failed: {insertResponse.message} {insertResponse.error}");
            return;
        }
        // After successful insert, fetch latest players and refresh UI
        getPlayersResponse = await GenericApiClient.Instance.GetPlayers();
        scrollView?.Clear();
        if (getPlayersResponse != null)
        {
            Refresh(getPlayersResponse);
        }
    }

    private void OnDeleteClicked()
    {
        // Voeg hier de logica toe om een speler te verwijderen
        string idText = DeleteIDField?.value?.Trim();

      RoyalData.entries = RoyalData.entries.Where(entry => entry.ID.ToString() != idText).ToArray();

        DeleteIDField.value = string.Empty;
        Debug.Log("Clicked Delete");
        OnRefreshClicked();

    }
}

[System.Serializable]
public class playerData
{
    public entries[] entries;
}

[System.Serializable]
public class entries
{
    public int ID;
    public string username;
    public int score;
    public string favoriteUnit;
}