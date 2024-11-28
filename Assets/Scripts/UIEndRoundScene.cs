using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{

public void LoadLoginScene()
{
    SceneManager.LoadScene("TriviaSelectScene");
}
public void PreviousScene()
{
    // Destruir explícitamente las instancias de GameManager y UIManagment
    Destroy(GameManager.Instance.gameObject);
    Destroy(UIManagment.Instance.gameObject);

    // Cargar la escena anterior
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
}

}