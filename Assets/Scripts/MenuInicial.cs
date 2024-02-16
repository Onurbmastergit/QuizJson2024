using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public GameObject painelInicial;

    private void Start()
    {
        if (!GameManager.Instance.gameStarted)
        {
            painelInicial.SetActive(true);
            GameManager.Instance.gameStarted = true;
        } else painelInicial.SetActive(false);
    }

    public void Entrar()
    {
        painelInicial.SetActive(false);
    }

    public void Sair()
    {
        Debug.Log("Comando: sair da página web atual para a desejada");
        Application.OpenURL("https://www.example.com");
    }

    public void Voltar()
    {
        painelInicial.SetActive(true);
    }

    public void CasoSelecionado(int i)
    {
        if(GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.casoSelecionado = i;
        Debug.Log($"Caso Selecionado: {i}");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
