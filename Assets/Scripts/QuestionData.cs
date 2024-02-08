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
}