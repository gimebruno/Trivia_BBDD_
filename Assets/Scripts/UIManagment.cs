using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagment : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _categoryText;
    [SerializeField] TextMeshProUGUI _questionText;
    [SerializeField] TextMeshProUGUI timerText; 
     [SerializeField] TextMeshProUGUI _puntosText;// Referencia al TimerText

    string _correctAnswer;
    public Button[] _buttons = new Button[3];
    [SerializeField] Button _backButton;
    [SerializeField] Button _menuButton;
   

    private List<string> _answers = new List<string>();
    public bool queryCalled;
    private Color _originalButtonColor;
    public static UIManagment Instance { get; private set; }

    private float timerDuration = 15f; // Duración inicial del temporizador
    private float timerRemaining;     // Tiempo restante
    private bool isTimerRunning;     
    private int _score=0; // Indica si el temporizador está en marcha

    void Awake()
    {
        // Configura la instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para mantener el objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        queryCalled = false;
        _originalButtonColor = _buttons[0].GetComponent<Image>().color;
        ResetTimer(); // Resetea y arranca el temporizador al inicio
        ValidateButtons();
    }
void Update()
{ 
    UpdateTimer();
if (_categoryText != null)
{
    _categoryText.text = PlayerPrefs.GetString("SelectedTrivia", "Default Value");
}
else
{
    Debug.LogWarning("_categoryText no está asignado.");
}    _questionText.text = GameManager.Instance.responseList[GameManager.Instance.randomQuestionIndex].QuestionText;
    GameManager.Instance.CategoryAndQuestionQuery(queryCalled);

    if (GameManager.Instance == null)
    {
        Debug.LogError("GameManager.Instance es null.");
        return;
    }

    if (GameManager.Instance.responseList == null || GameManager.Instance.responseList.Count == 0)
    {
        Debug.LogError("responseList está vacío o no inicializado.");
        return;
    }

    if (GameManager.Instance.randomQuestionIndex < 0 || GameManager.Instance.randomQuestionIndex >= GameManager.Instance.responseList.Count)
    {
        Debug.LogError("randomQuestionIndex no tiene un valor válido.");
        return;
    }

    // Actualiza los textos si todo es válido
    _categoryText.text = PlayerPrefs.GetString("SelectedTrivia");
    _questionText.text = GameManager.Instance.responseList[GameManager.Instance.randomQuestionIndex].QuestionText;

    // Llama a la función si es necesario
    GameManager.Instance.CategoryAndQuestionQuery(queryCalled);
}

    public void OnButtonClick(int buttonIndex)
    {
        StopTimer(); // Pausa el temporizador mientras procesa la respuesta

        string selectedAnswer = _buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        _correctAnswer = GameManager.Instance.responseList[GameManager.Instance.randomQuestionIndex].CorrectOption;

        if (selectedAnswer == _correctAnswer)
        {
            Debug.Log("¡Respuesta correcta!");
            ChangeButtonColor(buttonIndex, Color.green);
            Invoke("RestoreButtonColor", 2f);
            GameManager.Instance._answers.Clear();
            AddPoints(50);
            Invoke("NextAnswer", 2f);
        }
        else
        {
            Debug.Log("Respuesta incorrecta. Inténtalo de nuevo.");
            GameManager.Instance.LoseLife(); // Resta una vida al jugador
            ChangeButtonColor(buttonIndex, Color.red);
            Invoke("RestoreButtonColor", 2f);
            GameManager.Instance._answers.Clear();
             Invoke("NextAnswer", 2f);
        }
    }

    private void ChangeButtonColor(int buttonIndex, Color color)
    {
        Image buttonImage = _buttons[buttonIndex].GetComponent<Image>();
        buttonImage.color = color;
    }

    private void RestoreButtonColor()
    {
        foreach (Button button in _buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = _originalButtonColor;
        }
    }
  public void AddPoints(int points)
    {
        _score += points;
        UpdateScoreDisplay();
        
    }
    private void NextAnswer()
    {
        queryCalled = false;
        ResetTimer(); // Reinicia el temporizador para la siguiente pregunta
    }

  public void PreviousScene()
{
    if (GameManager.Instance != null)
        Destroy(GameManager.Instance.gameObject);

    if (UIManagment.Instance != null)
        Destroy(UIManagment.Instance.gameObject);

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}

    private void ResetTimer()
    {
        timerRemaining = timerDuration;
        isTimerRunning = true;
        UpdateTimerText();
    }

    private void UpdateTimer()
    {
        if (!isTimerRunning) return;

        timerRemaining -= Time.deltaTime;

        if (timerRemaining <= 0)
        {
            timerRemaining = 0;
            isTimerRunning = false;

            // Tiempo agotado: resta una vida y muestra la respuesta correcta
            Debug.Log("¡Tiempo agotado!");
            ShowCorrectAnswer();
            GameManager.Instance.LoseLife();
            Invoke("NextAnswer", 2f);
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.CeilToInt(timerRemaining).ToString(); // Actualiza el texto en pantalla
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private void ShowCorrectAnswer()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            string buttonText = _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text;
            if (buttonText == _correctAnswer)
            {
                ChangeButtonColor(i, Color.green);
                break;
            }
        }
    }
 private void UpdateScoreDisplay()
    {
       _puntosText.text = _score.ToString();
    }
  public int GetCurrentScore()
    {
        return _score;

}
private void ValidateButtons()
{
    if (_buttons == null || _buttons.Length == 0)
    {
        Debug.LogError("El array de botones no está asignado o está vacío.");
        return;
    }

    for (int i = 0; i < _buttons.Length; i++)
    {
        if (_buttons[i] == null)
        {
            Debug.LogError($"Botón {i} no está asignado en el Inspector.");
        }
    }
}



}