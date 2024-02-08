using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;

// Classe que define um editor de dados do jogo
public class GameDataEditor : EditorWindow
{
    // O objeto de dados do jogo
    public GameData gameData;

    // O caminho do arquivo de dados do jogo
    private string gameDataFilePath = "/StreamingAssets/data.json";

    // Método de inicialização para criar a janela do editor
    [MenuItem ("Window/Game Data Editor")]
    static void Init()
    {
        // Cria uma nova instância do GameDataEditor e a exibe como uma janela
        GameDataEditor window = (GameDataEditor)EditorWindow.GetWindow(typeof(GameDataEditor));
        window.Show();    
    }

    // Método para desenhar a interface do usuário do editor
    private void OnGUI()
    {
        // Verifica se existe um objeto de dados do jogo
        if (gameData != null)
        {
            // Cria um objeto serializado para o GameDataEditor
            SerializedObject serializedObject = new SerializedObject(this);
            // Encontra a propriedade do gameData no objeto serializado
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");

            // Desenha uma interface para a propriedade serializada do gameData
            EditorGUILayout.PropertyField(serializedProperty, true);

            // Aplica quaisquer modificações feitas ao objeto serializado
            serializedObject.ApplyModifiedProperties();

            // Botão para salvar os dados do jogo
            if (GUILayout.Button("Save Data"))
            {
                SaveGameData();     
            }
        }

        // Botão para carregar os dados do jogo
        if (GUILayout.Button("Load Data"))
        {
            LoadGameData();
        }
    }

    // Método para carregar os dados do jogo a partir do arquivo
    private void LoadGameData()
    {
        // Constrói o caminho completo do arquivo de dados do jogo
        string filePath = Application.dataPath + gameDataFilePath;

        // Verifica se o arquivo de dados do jogo existe
        if (File.Exists(filePath))
        {
            // Lê o conteúdo do arquivo como uma string JSON
            string dataAsJson = File.ReadAllText(filePath);
            // Converte a string JSON em um objeto GameData usando a classe de utilitário JsonUtility
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            // Se o arquivo não existe, cria um novo objeto GameData
            gameData = new GameData();
        }
    } 

    // Método para salvar os dados do jogo no arquivo
    private void SaveGameData()
    {
        // Converte o objeto gameData em uma string JSON
        string dataAsJson = JsonUtility.ToJson(gameData);
        // Constrói o caminho completo do arquivo de dados do jogo
        string filePath = Application.dataPath + gameDataFilePath;
        // Escreve a string JSON no arquivo
        File.WriteAllText(filePath, dataAsJson); 
    }
}