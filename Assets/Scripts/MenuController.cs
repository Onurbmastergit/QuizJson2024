using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Classe responsável pelo controle do menu
public class MenuController : MonoBehaviour
{
    // Referência ao controlador de dados
    private DataController data;

    // Método chamado quando o objeto é inicializado
    void Start()
    {
        // Encontra o DataController na cena
        data = FindObjectOfType<DataController>();
    }

    // Método para iniciar o jogo com a rodada especificada
    public void StartGame(int round)
    {
        // Define os dados da rodada no controlador de dados
        data.SetRoundData(round);
        // Carrega a cena do jogo
        SceneManager.LoadScene("Game");
    }
}