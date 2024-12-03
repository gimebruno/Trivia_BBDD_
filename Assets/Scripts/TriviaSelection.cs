using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriviaSelectionWithButtons : MonoBehaviour
{
    string supabaseUrl = "https://jnflucstnwobxmoefgdk.supabase.co"; // COMPLETAR
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImpuZmx1Y3N0bndvYnhtb2VmZ2RrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIyMjAwNzQsImV4cCI6MjA0Nzc5NjA3NH0.2O-1NG2AFAxkpKbKIv69XpGxRnZlfoH5ChZoWENtGNw"; // COMPLETAR

    Supabase.Client clientSupabase;
    List<trivia> trivias = new List<trivia>();

    [SerializeField] private List<TMP_Text> buttonLabels; // Textos de los botones
    [SerializeField] private List<UnityEngine.UI.Button> categoryButtons; // Referencia a los 6 botones


    async void Start()
    {
        clientSupabase = new Supabase.Client(supabaseUrl, supabaseKey);

        await SelectTrivias();
        AssignCategoriesToButtons();
       

    }

    async Task SelectTrivias()
    {
        var response = await clientSupabase
            .From<trivia>()
            .Select("*")
            .Get();

        if (response != null)
        {
            trivias = response.Models;
        }
    }

    void AssignCategoriesToButtons()
    {
        // Asegúrate de que haya exactamente 6 categorías
        if (trivias.Count < 6)
        {
            Debug.LogError("No hay suficientes categorías en la base de datos.");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            string category = trivias[i].category;
            buttonLabels[i].text = category;

        int triviaId = trivias[i].id;
        categoryButtons[i].onClick.AddListener(() => OnCategoryButtonClicked(category, triviaId));
        }
    }

    void OnCategoryButtonClicked(string category, int index)
    {
        PlayerPrefs.SetInt("SelectedIndex", index);
        PlayerPrefs.SetString("SelectedTrivia", category);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void rankingButton()
{
    SceneManager.LoadScene("Ranking");
      
}
}
