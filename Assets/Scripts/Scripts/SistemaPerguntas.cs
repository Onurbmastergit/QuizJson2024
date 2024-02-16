using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SistemaPerguntas : MonoBehaviour
{
    private JsonPerguntasReader jsonPerguntasReader;

    public TextMeshProUGUI perguntaText;
    public TextMeshProUGUI comentarioText;

    public TextMeshProUGUI respostaOpcao0;
    public TextMeshProUGUI respostaOpcao1;
    public TextMeshProUGUI respostaOpcao2;
    public TextMeshProUGUI respostaOpcao3;
    public TextMeshProUGUI respostaOpcao4;

    int perguntaSelecionada;
    List<int> perguntasUsadas = new List<int>();

    public GameObject perguntas;
    public GameObject respostas;
    public GameObject comentario;
    public GameObject pista;

    public int timer = 60;
    public TextMeshProUGUI timerText;

    void Start()
    {
        jsonPerguntasReader = FindObjectOfType<JsonPerguntasReader>();

        StartCoroutine(Timer());
        GerarPergunta();
    }

    public void GerarPergunta()
    {
        Aleatorizador();
        timer = 60;

        var perguntaAtual = jsonPerguntasReader.listaPerguntas[perguntaSelecionada];

        perguntaText.text = perguntaAtual.pergunta.ToString();
        comentarioText.text = perguntaAtual.comentario.ToString();

        respostaOpcao0.text = perguntaAtual.opcoes[0].ToString();
        respostaOpcao1.text = perguntaAtual.opcoes[1].ToString();
        respostaOpcao2.text = perguntaAtual.opcoes[2].ToString();
        respostaOpcao3.text = perguntaAtual.opcoes[3].ToString();
        respostaOpcao4.text = perguntaAtual.opcoes[4].ToString();

    }

    void Aleatorizador()
    {
        if (perguntasUsadas.Count == jsonPerguntasReader.listaPerguntas.Count)
        {
            perguntasUsadas.Clear();
        }

        perguntaSelecionada = UnityEngine.Random.Range(0, jsonPerguntasReader.listaPerguntas.Count - 1);

        if (perguntasUsadas.Contains(perguntaSelecionada))
        {
            Aleatorizador();
            return;
        }

        perguntasUsadas.Add(perguntaSelecionada);
    }

    public void BotaoResposta(int alternativa)
    {
        var respostaCorreta = jsonPerguntasReader.listaPerguntas[perguntaSelecionada].resposta_correta;

        GameManager.Instance.perguntasRespondidas++;

        if (respostaCorreta == alternativa)
        {
            GameManager.Instance.perguntasAcertadas++;
            CaseManager.Instance.playerScore++;

            if (CaseManager.Instance.playerScore >= 10)
            {
                CaseManager.Instance.playerScore = 0;
                perguntas.SetActive(false);
                pista.SetActive(true);
            }

            GerarPergunta();
        }
        else RespostaErrada();
    }

    void RespostaErrada()
    {
        respostas.SetActive(false);
        comentario.SetActive(true);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(timer);

            if (timer >= 0) timerText.text = timer.ToString();
            else if (timer == -1) RespostaErrada();

            timer--;
        }
    }

    public void FecharComentario()
    {
        respostas.SetActive(true);
        comentario.SetActive(false);
        GerarPergunta();
    }
}
