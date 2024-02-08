using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Classe que representa um botão de resposta em um jogo
public class AnswerButton : MonoBehaviour
{
    // Referência ao componente de texto usado para exibir a resposta
    public TextMeshProUGUI textoDaResposta;

    // Os dados da resposta associados a este botão
    private AnswerData answerData;

    // Referência ao controlador do jogo
    private GameController gameController;

    // Método chamado quando o objeto é inicializado
    void Start()
    {
        // Encontra o GameController na cena e o armazena para uso posterior
        gameController = FindObjectOfType<GameController>();
    }

    // Método para configurar o botão de resposta com os dados fornecidos
    public void Setuo(AnswerData data)
    {
        // Armazena os dados da resposta fornecidos
        answerData = data;
        
        // Define o texto do botão de resposta como o texto fornecido nos dados da resposta
        textoDaResposta.text = answerData.textoResposta;
    }

    // Método chamado quando o botão de resposta é clicado
    public void HandleClick()
    {
        // Informa ao GameController que este botão de resposta foi clicado, passando se a resposta é correta ou não
        gameController.AnswerButtonClicked(answerData.estaCorreta);
    }
}