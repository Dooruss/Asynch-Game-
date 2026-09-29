using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDisplayWindow : MonoBehaviour
{
    public playerData RoyalData;
    // Een UI Document voeg je toe als een VisualTreeAsset in de inspector.
    [SerializeField] private VisualTreeAsset rowAsset;
    [TextArea(4, 10)] public string json;

    private PanelRenderer panelRenderer;
    private ScrollView scrollView;
    private Button refreshButton;
    private Button insertButton;
    private Button updateButton;
    private Button deleteButton;

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

    private void OnRefreshClicked()
    {
        scrollView.Clear();

        for (int i = 0; i < RoyalData.entries.Length; i++)
        {

            // Maak een nieuwe rij aan door de VisualTreeAsset te klonen
            entries DataNumber = RoyalData.entries[i];
            VisualElement row = rowAsset.CloneTree();
            row.Q<Label>("FirstLabel").text = DataNumber.ID.ToString();
            row.Q<Label>("SecondLabel").text = DataNumber.username;
            row.Q<Label>("ThirdLabel").text = DataNumber.score.ToString();
            row.Q<Label>("FourthLabel").text = DataNumber.favoriteUnit;

            // Voeg de rij toe aan de ScrollView
            scrollView.Add(row);
        }
    }

    private void OnInsertClicked()
    {
        // Voeg hier de logica toe om een nieuwe speler in te voegen
    }

    private void OnUpdateClicked()
    {
        // Voeg hier de logica toe om een speler bij te werken
    }

    private void OnDeleteClicked()
    {
        // Voeg hier de logica toe om een speler te verwijderen
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