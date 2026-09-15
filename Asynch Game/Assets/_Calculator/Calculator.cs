using UnityEngine;
using UnityEngine.UIElements;

public class Calculator : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    // Referenties naar de UI-elementen die we vanuit C# willen gebruiken.
    private int firstNumber;
    private int secondNumber;
    private int Results;
    //Other buttons
    private Button submitButton;
    private Button PlusButton;
    private Button MinusButton;
    private Button DivideButton;
    private Button TimesButton;
    //Numbers
    private Button Number_1Button;
    private Button Number_2Button;
    private Button Number_3Button;
    private Button Number_4Button;
    private Button Number_5Button;
    private Button Number_6Button;
    private Button Number_7Button;
    private Button Number_8Button;
    private Button Number_9Button;
    private Button Number_0Button;

    private enum Operator { None, Add, Subtract, Multiply, Divide }
    private Operator currentOperator = Operator.None;

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
        Number_2Button = rootElement.Q<Button>("Number_2Button");
        Number_3Button = rootElement.Q<Button>("Number_3Button");
        Number_4Button = rootElement.Q<Button>("Number_4Button");
        Number_5Button = rootElement.Q<Button>("Number_5Button");
        Number_6Button = rootElement.Q<Button>("Number_6Button");
        Number_7Button = rootElement.Q<Button>("Number_7Button");
        Number_8Button = rootElement.Q<Button>("Number_8Button");
        Number_9Button = rootElement.Q<Button>("Number_9Button");
        Number_0Button = rootElement.Q<Button>("Number_0Button");

        // Verwijder eerst eventuele oude callbacks om te voorkomen dat dezelfde
        // callback meerdere keren geregistreerd staat als de UI opnieuw wordt geladen.
        UnregisterCallbacks();

        // Registreer de callbacks op de zojuist gevonden UI-elementen.
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        // Voer OnSubmitButtonClicked uit wanneer op de button wordt geklikt.
        if (submitButton != null) submitButton.clicked += OnSubmitButtonClicked;
        if (PlusButton != null) PlusButton.clicked += OnPlusButtonClicked;
        if (MinusButton != null) MinusButton.clicked += OnMinusButtonClicked;
        if (DivideButton != null) DivideButton.clicked += OnDivideButtonClicked;
        if (TimesButton != null) TimesButton.clicked += OnTimesButtonClicked;
        if (Number_1Button != null) Number_1Button.clicked += OnNumber_1ButtonClicked;
        if (Number_2Button != null) Number_2Button.clicked += OnNumber_2ButtonClicked;
        if (Number_3Button != null) Number_3Button.clicked += OnNumber_3ButtonClicked;
        if (Number_4Button != null) Number_4Button.clicked += OnNumber_4ButtonClicked;
        if (Number_5Button != null) Number_5Button.clicked += OnNumber_5ButtonClicked;
        if (Number_6Button != null) Number_6Button.clicked += OnNumber_6ButtonClicked;
        if (Number_7Button != null) Number_7Button.clicked += OnNumber_7ButtonClicked;
        if (Number_8Button != null) Number_8Button.clicked += OnNumber_8ButtonClicked;
        if (Number_9Button != null) Number_9Button.clicked += OnNumber_9ButtonClicked;
        if (Number_0Button != null) Number_0Button.clicked += OnNumber_0ButtonClicked;
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
        if (Number_2Button != null) { Number_2Button.clicked -= OnNumber_2ButtonClicked; }
        if (Number_3Button != null) { Number_3Button.clicked -= OnNumber_3ButtonClicked; }
        if (Number_4Button != null) { Number_4Button.clicked -= OnNumber_4ButtonClicked; }
        if (Number_5Button != null) { Number_5Button.clicked -= OnNumber_5ButtonClicked; }
        if (Number_6Button != null) { Number_6Button.clicked -= OnNumber_6ButtonClicked; }
        if (Number_7Button != null) { Number_7Button.clicked -= OnNumber_7ButtonClicked; }
        if (Number_8Button != null) { Number_8Button.clicked -= OnNumber_8ButtonClicked; }
        if (Number_9Button != null) { Number_9Button.clicked -= OnNumber_9ButtonClicked; }
        if (Number_0Button != null) { Number_0Button.clicked -= OnNumber_0ButtonClicked; }
        if (PlusButton != null) { PlusButton.clicked -= OnPlusButtonClicked; }
        if (MinusButton != null) { MinusButton.clicked -= OnMinusButtonClicked; }
        if (DivideButton != null) { DivideButton.clicked -= OnDivideButtonClicked; }
        if (TimesButton != null) { TimesButton.clicked -= OnTimesButtonClicked; }
    }
    private void OnSubmitButtonClicked()
    {
        // Bereken en toon resultaat op basis van de gekozen operator.
        switch (currentOperator)
        {
            case Operator.Add:
                Results = firstNumber + secondNumber;
                break;
            case Operator.Subtract:
                Results = firstNumber - secondNumber;
                break;
            case Operator.Divide:
                Results = firstNumber / secondNumber;
                break;
            case Operator.Multiply:
                Results = firstNumber * secondNumber;
                break;
            default:
                Results = firstNumber;
                break;
        }

        Debug.Log($"The answer equals: {Results}");

        // Reset state zodat gebruiker opnieuw kan rekenen
        firstNumber = 0;
        secondNumber = 0;
        currentOperator = Operator.None;
    }

    #region // Numbers
    private void OnNumber_1ButtonClicked()
    {
        // Als er al een operator gekozen is, wordt dit het tweede getal.
        if (currentOperator != Operator.None) { secondNumber = 1; }
        else { firstNumber = 1; }
    }
    private void OnNumber_2ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 2; }
        else { firstNumber = 2; }
    }
    private void OnNumber_3ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 3; }
        else { firstNumber = 3; }
    }
    private void OnNumber_4ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 4; }
        else { firstNumber = 4; }
    }
    private void OnNumber_5ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 5; }
        else { firstNumber = 5; }
    }
    private void OnNumber_6ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 6; }
        else { firstNumber = 6; }
    }
    private void OnNumber_7ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 7; }
        else { firstNumber = 7; }
    }
    private void OnNumber_8ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 8; }
        else { firstNumber = 8; }
    }
    private void OnNumber_9ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 9; }
        else { firstNumber = 9; }
    }
    private void OnNumber_0ButtonClicked()
    {
        if (currentOperator != Operator.None) { secondNumber = 0; }
        else { firstNumber = 0; }
    }
    #endregion

    #region // Plus , min etc.
    // Stel de operator in op optellen. Het resultaat wordt pas berekend
    // wanneer op Submit wordt gedrukt.
    private void OnPlusButtonClicked()
    {
        currentOperator = Operator.Add;
    }
    private void OnMinusButtonClicked()
    {
        currentOperator = Operator.Subtract;
    }
    private void OnDivideButtonClicked()
    {
        currentOperator = Operator.Divide;
    }
    private void OnTimesButtonClicked()
    {
        currentOperator = Operator.Multiply;
    }
    #endregion
}
