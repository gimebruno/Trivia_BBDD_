using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Supabase;

public class UIEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _finalScoreText; // Referencia al TextMeshPro para mostrar el puntaje

   string supabaseUrl = "https://jnflucstnwobxmoefgdk.supabase.co"; // COMPLETAR
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpuZmx1Y3N0bndvYnhtb2VmZ2RrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIyMjAwNzQsImV4cCI6MjA0Nzc5NjA3NH0.2O-1NG2AFAxkpKbKIv69XpGxRnZlfoH5ChZoWENtGNw"; // COMPLETAR

    Supabase.Client clientSupabase;

    void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);
        
        int finalScore = UIManagment.Instance.GetCurrentScore();  
        _finalScoreText.text =  finalScore.ToString();

        InsertarPuntaje(finalScore);
    }

    public async void InsertarPuntaje(int finalScore)
    {
        // Inicializa el cliente Supabase
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        // Consulta el último ID de la tabla 'score'
        var ultimoIdResultado = await clientSupabase
            .From<score>()
            .Select("id")
            .Order(score => score.id, Postgrest.Constants.Ordering.Descending)
            .Get();

        int nuevoId = 1; // Valor predeterminado si la tabla está vacía

        if (ultimoIdResultado.Models.Count > 0)
        {
            // Obtén el último ID y calcula el nuevo
            nuevoId = ultimoIdResultado.Models[0].id + 1;
        }

        // Crear el nuevo puntaje con el ID calculado
        var nuevoPuntaje = new score
        {
            id = nuevoId,
            usuario_id = SupabaseManager.currentUserId, // Usamos la variable estática del SupabaseManager
           
            puntaje = finalScore // Puntaje final obtenido
        };

        // Insertar el nuevo puntaje
        var resultado = await clientSupabase
            .From<score>()
            .Insert(new[] { nuevoPuntaje });

        // Verificar si la inserción fue exitosa
        if (resultado.ResponseMessage.IsSuccessStatusCode)
        {
            Debug.Log("Puntaje insertado correctamente");
        }
        else
        {
            Debug.LogError("Error al insertar puntaje: " + resultado.ResponseMessage.Content);
        }
    }
 
 public void LoadTriviaScene()
    {
        // Destruir las instancias solo si aún no han sido destruidas
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (UIManagment.Instance != null)
        {
            Destroy(UIManagment.Instance.gameObject);
        }

        // Cargar la escena de selección de trivia
        SceneManager.LoadScene("TriviaSelectScene");
    }

}
