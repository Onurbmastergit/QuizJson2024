using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Threading;
using static GameManager;

public class SistemaPerguntas : MonoBehaviour
{
    private JsonPerguntasReader jsonPerguntasReader;

    public TextMeshProUGUI score;

    public TextMeshProUGUI perguntaText;
    public TextMeshProUGUI comentarioText;

    public List<TextMeshProUGUI> listaRespostas = new List<TextMeshProUGUI>();

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

        List<int> valoresDisponiveis = new List<int> { 0, 1, 2, 3, 4 };

        for (int i = 0; i < valoresDisponiveis.Count; i++)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, valoresDisponiveis.Count);
            int valorAleatorio = valoresDisponiveis[indiceAleatorio];
            valoresDisponiveis.RemoveAt(indiceAleatorio);

            listaRespostas[i].text = perguntaAtual.opcoes[valorAleatorio].resposta.ToString();
        }
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
                CaseManager.Instance.totalPistas++;

                if (GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas == "")
                {
                    GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas += $"{CaseManager.Instance.localAtual}";
                }
                else
                {
                    GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas += $",{CaseManager.Instance.localAtual}";
                }

                Debug.Log($"Atualizando save Caso: {GameManager.Instance.casos[CaseManager.Instance.indexCasoID].CasoID} | PistasDesbloqueadas: {GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas}");
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
