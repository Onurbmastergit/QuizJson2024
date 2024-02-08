using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Classe que define os dados de uma resposta em um jogo
[System.Serializable]
public class AnswerData 
{
    // O texto da resposta
    public string textoResposta;
    
    // Um valor booleano que indica se a resposta está correta ou não
    public bool estaCorreta;
}