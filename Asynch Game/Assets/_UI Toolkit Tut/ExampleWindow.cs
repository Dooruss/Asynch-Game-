using UnityEngine;
using UnityEngine.UIElements;

public class ExampleWindow : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    // Referenties naar de UI-elementen die we vanuit C# willen gebruiken.
    private TextField nameField;
    private Button submitButton;

    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();

        // Registreer een callback die wordt uitgevoerd wanneer de UI
        // is geladen of opnieuw wordt geladen.
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        // Stop met luisteren naar het laden of opnieuw laden van de UI.
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        // Verwijder ook onze callbacks van de UI-elementen.
        UnregisterCallbacks();
    }

    // Deze methode wordt aangeroepen wanneer de UI door de PanelRenderer is geladen.
    // rootElement is het bovenste element van onze geladen UXML-layout.
    private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement, int version)
    {
        // Q<T>() zoekt in de UI naar een element van het opgegeven type
        // en met de opgegeven naam.
        // Deze namen hebben we eerder in de UI Builder ingesteld.
        nameField = rootElement.Q<TextField>("NameField");
        submitButton = rootElement.Q<Button>("SubmitButton");

        // Verwijder eerst eventuele oude callbacks om te voorkomen dat dezelfde
        // callback meerdere keren geregistreerd staat als de UI opnieuw wordt geladen.
        UnregisterCallbacks();

        // Registreer de callbacks op de zojuist gevonden UI-elementen.
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        // Voer OnSubmitButtonClicked uit wanneer op de button wordt geklikt.
        submitButton.clicked += OnSubmitButtonClicked;
    }

    private void UnregisterCallbacks()
    {
        // Controleer eerst of de button al gevonden en opgeslagen is.
        if (submitButton != null)
        {
            // Verwijder de eerder geregistreerde callback van de button.
            submitButton.clicked -= OnSubmitButtonClicked;
        }
    }

    private void OnSubmitButtonClicked()
    {
        // .value bevat de huidige tekst die de gebruiker
        // in het TextField heeft ingevuld.
        Debug.Log($"Submit button clicked with name: {nameField.value}");
    }
}