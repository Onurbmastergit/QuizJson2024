using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

// Classe responsável pelo controle dos dados do jogo
public class DataController : MonoBehaviour
{
    // Array que armazena todos os dados das rodadas do jogo
    private RoundData[] todasAsRodadas;

    // Índice da rodada atual
    private int rodadaIndex;

    // Pontuação mais alta do jogador
    private int playerHighScore;

    // Nome do arquivo de dados do jogo
    private string gameDataFileName = "data.json";

    // Método chamado quando o objeto é inicializado
    void Start()
    {
        // Mantém este objeto ativo entre as cenas
        DontDestroyOnLoad(gameObject);

        // Carrega os dados do jogo
        LoadGameData();

        // Carrega a cena do menu
        SceneManager.LoadScene("Menu");
    }

    // Método para definir os dados da rodada atual
    public void SetRoundData(int round)
    {
        rodadaIndex = round;
    }

    // Método para obter os dados da rodada atual
    public RoundData GetCurrentRoundData()
    {
        return todasAsRodadas[rodadaIndex];
    }

    // Método para carregar os dados do jogo
    private void LoadGameData()
    {
        // Constrói o caminho completo do arquivo de dados do jogo
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        // Verifica se o arquivo de dados do jogo existe
        if (File.Exists(filePath))
        {
            // Lê o conteúdo do arquivo como uma string JSON
            string dataAsJson = File.ReadAllText(filePath);
            // Converte a string JSON em um objeto GameData usando a classe de utilitário JsonUtility
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            // Atribui os dados de todas as rodadas carregadas aos dados armazenados nesta classe
            todasAsRodadas = loadedData.todasAsRodadas;    
        }
        else
        {
            // Se o arquivo não existe, exibe uma mensagem de erro
            Debug.LogError("Não foi possível carregar dados!!!");
        }
    }

    // Método para definir uma nova pontuação mais alta do jogador
    public void EnviarNovoHighScore(int newScore)
    {
        playerHighScore = newScore;
        // Salva o progresso do jogador
        SavePlayerProgress();
    }

    // Método para obter a pontuação mais alta do jogador
    public int GetHighScore()
    {
        return playerHighScore;
    }

    // Método para carregar o progresso do jogador
    private void LoadPlayerProgress()
    {
        // Verifica se há uma pontuação mais alta salva no PlayerPrefs
        if (PlayerPrefs.HasKey("highScore"))
        {
            playerHighScore = PlayerPrefs.GetInt("highScore");
        }
    }

    // Método para salvar o progresso do jogador
    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highScore", playerHighScore);
    }
}