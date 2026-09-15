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

        UnregisterCallbacks();
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        refreshButton.clicked += OnRefreshClicked;
    }

    private void UnregisterCallbacks()
    {
        if (refreshButton != null)
        {
            refreshButton.clicked -= OnRefreshClicked;
        }
    }

    private void OnRefreshClicked()
    {
        scrollView.Clear();

        for (int i = 0; i < RoyalData.entries.Length; i++)
        {

            // Maak een nieuwe rij aan door de VisualTreeAsset te klonen
            entries DataNumber = RoyalData.entries[i];
            VisualElement row = rowAsset.CloneTree();
            row.Q<Label>("FirstLabel").text = DataNumber.username;
            row.Q<Label>("SecondLabel").text = DataNumber.score.ToString();
            row.Q<Label>("ThirdLabel").text = DataNumber.favoriteUnit;

            // Voeg de rij toe aan de ScrollView
            scrollView.Add(row);
        }
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
    public string username;
    public int score;
    public string favoriteUnit;
}