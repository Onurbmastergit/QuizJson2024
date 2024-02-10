using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe que define os dados de uma pergunta em um jogo
[System.Serializable]
public class QuestionData
{
    // O texto da pergunta
    public string textoDaPergunta;

    // Um array de objetos AnswerData que representam as respostas possíveis para a pergunta
    public AnswerData[] respostas;

    // Método para embaralhar as respostas
    public void ShuffleAnswers()
    {
        // Embaralha as respostas usando o algoritmo de Fisher-Yates
        for (int i = respostas.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            AnswerData temp = respostas[i];
            respostas[i] = respostas[randomIndex];
            respostas[randomIndex] = temp;
        }
    }
}