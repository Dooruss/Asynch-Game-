using UnityEngine;
using UnityEngine.UIElements;

public class CreateAccountScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoginWindow;
    private PanelRenderer panelRenderer;
    private Button BackButton;
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
        BackButton = rootElement.Q<Button>("Button_back");

        UnregisterCallbacks();
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        BackButton.clicked += OnBackClicked;
    }

    private void UnregisterCallbacks()
    {
        if (BackButton != null)
        {
            BackButton.clicked -= OnBackClicked;
        }
    }

    private void OnBackClicked()
    {
        LoginWindow.SetActive(true);
        gameObject.SetActive(false);
    }
}