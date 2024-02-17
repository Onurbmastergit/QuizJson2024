using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Threading;

public class SistemaPerguntas : MonoBehaviour
{
    private JsonPerguntasReader jsonPerguntasReader;

    public TextMeshProUGUI score;

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

    public float timer = 61;
    private int segundosAnteriores = 0;
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

        timer = 61;

        var perguntaAtual = jsonPerguntasReader.listaPerguntas[perguntaSelecionada];

        perguntaText.text = perguntaAtual.pergunta.ToString();
        comentarioText.text = perguntaAtual.comentario.ToString();

        respostaOpcao0.text = perguntaAtual.opcoes[0].resposta.ToString();
        respostaOpcao1.text = perguntaAtual.opcoes[1].resposta.ToString();
        respostaOpcao2.text = perguntaAtual.opcoes[2].resposta.ToString();
        respostaOpcao3.text = perguntaAtual.opcoes[3].resposta.ToString();
        respostaOpcao4.text = perguntaAtual.opcoes[4].resposta.ToString();

    }

    void Aleatorizador()
    {
        if (perguntasUsadas.Count == jsonPerguntasReader.listaPerguntas.Count - 1)
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
        Debug.Log($"Perguntas usadas: {perguntasUsadas.Count} / Perguntas total: {jsonPerguntasReader.listaPerguntas.Count}");
    }

    void RespostaErrada()
    {
        timer = -1;
        respostas.SetActive(false);
        comentario.SetActive(true);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            if (CaseManager.Instance.rodadaAtiva)
            {
                timer -= Time.fixedDeltaTime;
            }

            // Verifica se houve uma mudança nos segundos
            int segundosAtuais = Mathf.FloorToInt(timer);
            if (segundosAtuais != segundosAnteriores && segundosAtuais > 0)
            {
                timerText.text = segundosAtuais.ToString();
                segundosAnteriores = segundosAtuais;
            }

            // Verifica se o tempo acabou
            if (segundosAtuais == 0f)
            {
                Debug.Log("Tempo esgotado!");
                RespostaErrada();
            } 
            // Verifica se a Resposta foi errada e zera o contador
            else if (segundosAtuais < 0)
            {
                timerText.text = "0";
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void BotaoResposta(string alternativa)
    {
        var respostaCorreta = jsonPerguntasReader.listaPerguntas[perguntaSelecionada].resposta_correta;

        GameManager.Instance.perguntasRespondidas++;

        Thread.Sleep(250);

        if (respostaCorreta == alternativa)
        {
            Thread.Sleep(250);
            GameManager.Instance.perguntasAcertadas++;
            CaseManager.Instance.playerScore++;
            score.text = score.text + "* ";

            if (CaseManager.Instance.playerScore >= 10)
            {
                CaseManager.Instance.playerScore = 0;
                score.text = "";
                perguntas.SetActive(false);
                pista.SetActive(true);
            }

            GerarPergunta();
        }
        else RespostaErrada();
    }

    public void FecharComentario()
    {
        respostas.SetActive(true);
        comentario.SetActive(false);
        GerarPergunta();
    }
}
