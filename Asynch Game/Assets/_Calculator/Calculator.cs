using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text;
using UnityEngine.Networking;

[Serializable]
public class CalculateRequest
{
    public string action;
    public int numberA;
    public int numberB;
    public string operation;
}

[Serializable]
public class CalculateResponse
{
    public bool success;
    public int numberA;
    public int numberB;
    public string operation;
    public int result;
}

public class Calculator : MonoBehaviour
{
    private const string ApiUrl = "http://localhost/CalculatorCalculate.php";
    [SerializeField] private CalculateResponse response;
    private PanelRenderer panelRenderer;

    // Referenties naar de UI-elementen die we vanuit C# willen gebruiken.
    private int firstNumber;
    private int secondNumber;
    private int Results;
    //Other buttons
    private Label resultLabel;
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
        resultLabel = rootElement.Q<Label>("Result_Text");

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
    private async void OnSubmitButtonClicked()
    {
        string operationString = currentOperator switch
        {
            Operator.Add => "add",
            Operator.Subtract => "subtract",
            Operator.Multiply => "multiply",
            Operator.Divide => "divide",
            _ => "none"
        };

        CalculateRequest requestData = new CalculateRequest
        {
            action = "calculate",
            numberA = firstNumber,
            numberB = secondNumber,
            operation = operationString
        };

        string json = JsonUtility.ToJson(requestData);

        using UnityWebRequest request = new UnityWebRequest(ApiUrl, UnityWebRequest.kHttpVerbPOST);
        byte[] body = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Verstuur het request en wacht op de response
        await request.SendWebRequest();

        // Lees response en zet om naar object.
        string responseJson = request.downloadHandler.text;
        try
        {
            response = JsonUtility.FromJson<CalculateResponse>(responseJson);
            if (response != null)
            {
                Results = response.result;
                Debug.Log($"Server response: success={response.success}, result={response.result}");
                UpdateResultLabel();
            }
            else
            {
                Debug.LogWarning($"Failed");
            }
        }
        // wtf doet dit
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing response: {ex.Message}");
        }

        // Reset state zodat gebruiker opnieuw kan rekenen
        firstNumber = 0;
        secondNumber = 0;
        currentOperator = Operator.None;
        UpdateResultLabel();
    }

    #region // Numbers
    private void OnNumber_1ButtonClicked()
    {
        MaybeClearResponse();
        // Als er al een operator gekozen is, wordt dit het tweede getal.
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 1; }
        else { firstNumber = firstNumber * 10 + 1; }
        UpdateResultLabel();
    }
    private void OnNumber_2ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 2; }
        else { firstNumber = firstNumber * 10 + 2; }
        UpdateResultLabel();
    }
    private void OnNumber_3ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 3; }
        else { firstNumber = firstNumber * 10 + 3; }
        UpdateResultLabel();
    }
    private void OnNumber_4ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 4; }
        else { firstNumber = firstNumber * 10 + 4; }
        UpdateResultLabel();
    }
    private void OnNumber_5ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 5; }
        else { firstNumber = firstNumber * 10 + 5; }
        UpdateResultLabel();
    }
    private void OnNumber_6ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 6; }
        else { firstNumber = firstNumber * 10 + 6; }
        UpdateResultLabel();
    }
    private void OnNumber_7ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 7; }
        else { firstNumber = firstNumber * 10 + 7; }
        UpdateResultLabel();
    }
    private void OnNumber_8ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 8; }
        else { firstNumber = firstNumber * 10 + 8; }
        UpdateResultLabel();
    }
    private void OnNumber_9ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 9; }
        else { firstNumber = firstNumber * 10 + 9; }
        UpdateResultLabel();
    }
    private void OnNumber_0ButtonClicked()
    {
        MaybeClearResponse();
        if (currentOperator != Operator.None) { secondNumber = secondNumber * 10 + 0; }
        else { firstNumber = firstNumber * 10 + 0; }
        UpdateResultLabel();
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

    // Clear stored server response when the user starts typing a new number
    private void MaybeClearResponse()
    {
        if (response != null && response.success && currentOperator == Operator.None && firstNumber == 0 && secondNumber == 0)
        {
            response = null;
            Results = 0;
        }
    }

    // Update the result label to show the currently typed number plus the last result
    private void UpdateResultLabel()
    {
        if (resultLabel == null) return;

        // If there's a recent successful server response and the user is not typing a new input,
        // show the full calculation returned by the server.
        if (response != null && response.success && currentOperator == Operator.None && firstNumber == 0 && secondNumber == 0)
        {
            string op = GetSymbol(response.operation);
            resultLabel.text = $"{response.numberA} {op} {response.numberB} = {response.result}";
            return;
        }

        // If an operator is selected, show the current expression being typed.
        if (currentOperator != Operator.None)
        {
            string op = GetSymbol(currentOperator);
            resultLabel.text = $"{firstNumber} {op} {secondNumber}";
            return;
        }

        // Default: show the currently typed first number (or 0).
        resultLabel.text = firstNumber.ToString();
    }

    private string GetSymbol(Operator op)
    {
        return op switch
        {
            Operator.Add => "+",
            Operator.Subtract => "-",
            Operator.Multiply => "×",
            Operator.Divide => "/",
            _ => ""
        };
    }

    private string GetSymbol(string operation)
    {
        return operation switch
        {
            "add" => "+",
            "subtract" => "-",
            "multiply" => "×",
            "divide" => "/",
            _ => "?",
        };
    }
}
