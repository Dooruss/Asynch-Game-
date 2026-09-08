using UnityEngine;
using UnityEngine.UIElements;

public class Calculator : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    // Referenties naar de UI-elementen die we vanuit C# willen gebruiken.
    private int firstNumber;
    private int secondNumber;
    private int Results;
    private Button submitButton;
    private Button PlusButton;
    private Button MinusButton;
    private Button DivideButton;
    private Button TimesButton;
    private Button Number_1Button;

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
        submitButton = rootElement.Q<Button>("SubmitButton");

        PlusButton = rootElement.Q<Button>("PlusButton");
        MinusButton = rootElement.Q<Button>("MinusButton");
        DivideButton = rootElement.Q<Button>("DivideButton");
        TimesButton = rootElement.Q<Button>("TimesButton");
        Number_1Button = rootElement.Q<Button>("Number_1Button");

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
        PlusButton.clicked += OnPlusButtonClicked;
        Number_1Button.clicked += OnNumber_1ButtonClicked;
        //MinusButton.clicked += OnMinusButtonClicked;
        //DivideButton.clicked += OnDivideButtonClicked;
        //TimesButton.clicked += OnTimesButtonClicked;
    }

    private void UnregisterCallbacks()
    {
        // Controleer eerst of de button al gevonden en opgeslagen is.
        if (submitButton != null)
        {
            // Verwijder de eerder geregistreerde callback van de button.
            submitButton.clicked -= OnSubmitButtonClicked;
        }
        if (Number_1Button != null) { Number_1Button.clicked -= OnNumber_1ButtonClicked; }
        if (PlusButton != null) { PlusButton.clicked -= OnPlusButtonClicked; }
        //if (MinusButton != null) { MinusButton.clicked -= OnMinusButtonClicked; }
        //if (DivideButton != null) { DivideButton.clicked -= OnDivideButtonClicked; }
        //if (TimesButton != null) { TimesButton.clicked -= OnTimesButtonClicked; }
    }
    private void OnSubmitButtonClicked()
    {
        // Hier kun je de berekening uitvoeren en het resultaat tonen.
        // Bijvoorbeeld:
        Debug.Log($"The answer equals: {Results}");
    }

    private void OnNumber_1ButtonClicked()
    {
        firstNumber = 1;
    }

    private void OnPlusButtonClicked()
    {
        if (firstNumber != 0 && secondNumber != 0)
        {
            Results = firstNumber + secondNumber;
            firstNumber = 0;
            secondNumber = 0;
        }
    }
}
