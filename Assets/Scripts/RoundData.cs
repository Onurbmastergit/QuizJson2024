using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe que define os dados de uma rodada em um jogo
[System.Serializable]
public class RoundData 
{
    // O nome do tema da rodada
    public string nomeDoTema;
    
    // O limite de tempo para responder às perguntas da rodada
    public int limiteDeTempo;
    
    // O número de pontos concedidos por resposta correta na rodada
    public int pontosPorAcerto;
    
    // Um array de objetos QuestionData que representam as perguntas da rodada
    public QuestionData[] perguntas;
}