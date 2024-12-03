using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Supabase;
using System.Linq;
using System.Threading.Tasks;


public class Ranking : MonoBehaviour
{
    public GameObject[] rankButtons;  // Botones de ranking (en el inspector)
    public List<TMP_Text> usernameText;  // Lista de TextMeshPro para nombres de usuario
    public List<TMP_Text> scoreText; // Lista de TextMeshPro para puntajes
  
    string supabaseUrl = "https://jnflucstnwobxmoefgdk.supabase.co";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpuZmx1Y3N0bndvYnhtb2VmZ2RrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIyMjAwNzQsImV4cCI6MjA0Nzc5NjA3NH0.2O-1NG2AFAxkpKbKIv69XpGxRnZlfoH5ChZoWENtGNw"; // Mantén tu clave secreta
    Supabase.Client clientSupabase;
    List<usuarios> users = new List<usuarios>();
   List<score> attempts = new List<score>();

     async void Start()
    {
         clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey); 
        await LoadAttemptData();
        await LoadUser();
        UpdateRanking();
     
    }
        async Task LoadAttemptData()
    {
        var response = await clientSupabase
            .From<score>()
            .Select("*")
            .Get();

        if (response != null)
        {
            attempts = response.Models;
        }
    }

    async Task LoadUser()
    {
        var response = await clientSupabase
            .From<usuarios>()
            .Select("*")
            .Get();

        if (response != null)
        {
            users = response.Models;
        }
    }
    void UpdateRanking()
    {
        var sortedUsers = attempts.OrderByDescending(x => x.puntaje).Take(5).ToList();  // Solo los 3 mejores

        for (int i = 0; i < usernameText.Count; i++)
        {
            if (i < sortedUsers.Count)
            {
                var user = users.FirstOrDefault(u => u.id == sortedUsers[i].usuario_id);
                if (user != null)
                {
                    usernameText[i].text = user.username;         // Asignar nombre de usuario
                    scoreText[i].text = sortedUsers[i].puntaje.ToString(); // Asignar puntaje
                }
            }
            else
            {
                usernameText[i].text = "";  // Limpiar texto si no hay más usuarios
                scoreText[i].text = "";
            }
        }
    }

  

    public void MenuButton()
    {
        SceneManager.LoadScene("TriviaSelectScene");
    }
}
