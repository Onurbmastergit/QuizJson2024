using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestCaso : MonoBehaviour
{
    public GameObject painelWin;
    public GameObject painelOver;
    public GameObject caso;

    public void RepostaCerta() // Agora é público
    {
        caso.SetActive(false);
        painelWin.SetActive(true);
    }

    public void RepostaErrada() // Agora é público
    {
        caso.SetActive(false);
        painelOver.SetActive(true);
    }

    public void BackToMenu() // Agora é público
    {
        SceneManager.LoadScene("Menu");
    }
}
