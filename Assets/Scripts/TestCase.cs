using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TestCase : MonoBehaviour
{
    public GameObject painelWin;
    public GameObject painelOver;
    public GameObject caso;
    public GameObject coment;
    public static bool comentario;

    void Update()
    {
        if (comentario == true)
        {
            Comentario();
        }
    }
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
    public void ExitComen()
    {
        Thread.Sleep(1000);
        comentario = !comentario;
        coment.SetActive(comentario);
    }
    void Comentario()
    {
        coment.SetActive(comentario);
    }
}
