using UnityEngine;
using Supabase;
using Supabase.Interfaces;
using System.Threading.Tasks;
using Postgrest.Models;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText; 
    private int currentScore = 0; 

    string supabaseUrl = "https://jnflucstnwobxmoefgdk.supabase.co"; 
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpuZmx1Y3N0bndvYnhtb2VmZ2RrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIyMjAwNzQsImV4cCI6MjA0Nzc5NjA3NH0.2O-1NG2AFAxkpKbKIv69XpGxRnZlfoH5ChZoWENtGNw"; // Reemplazar con tu clave

    Supabase.Client clientSupabase;
    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject);
    }
}

    // Se llama al inicio
    void Start()
    {
        // Inicializa el cliente Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        // Mostrar el puntaje inicial
        UpdateScoreText();
    }

    // Este método se llama cada vez que el jugador responde correctamente
    public void AddPoints(int pointsToAdd)
    {
        currentScore += pointsToAdd;  
         Debug.Log("Puntaje actual: " + currentScore);// Sumar los puntos por respuesta correcta
        UpdateScoreText();  // Actualizar el puntaje en la UI
    }

    // Actualizar el texto de puntaje en la UI
    private void UpdateScoreText()
    {
        scoreText.text =  currentScore.ToString();
    }
    public async Task SaveScore(int userId)
{
    if (clientSupabase == null)
    {
        Debug.LogError("Supabase client not initialized");
        return;
    }

    var newScore = new score
    {
        usuario_id = userId,
        puntaje = currentScore
    };

    try
    {
        var response = await clientSupabase.From<score>().Insert(newScore);
        Debug.Log("Score saved successfully!");
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Error saving score: {ex.Message}");
    }
}

    
}