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

    public Image score;

    public TextMeshProUGUI perguntaText;
    public TextMeshProUGUI comentarioText;

    int botaoCorreto;

    int perguntaSelecionada;
    List<int> perguntasUsadas = new List<int>();

    public GameObject perguntas;
    public GameObject respostas;
    public GameObject comentario;
    public GameObject pista;

    public float timer = 61;
    private int segundosAnteriores = 0;
    public TextMeshProUGUI timerText;

    public Animator animatorCorreto;
    public Animator animatorErrado;
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

        #region Sistema para gerar e embaralhar as respostas
        // Destroi as alternativas passadas
        foreach (Transform child in respostas.transform)
        {
            Destroy(child.gameObject);
        }

        // Contador de numero de alternativas atraves do perguntas.json
        int count = perguntaAtual.opcoes.Count;

        List<int> valoresDisponiveis = new List<int>();
        for (int i = 0; i < count; i++)
        {
            valoresDisponiveis.Add(i);
        }

        // Aleatoriza a ordem da qual os botoes serao instanciados
        for (int i = 0; i < count; i++)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, valoresDisponiveis.Count);
            int valorAleatorio = valoresDisponiveis[indiceAleatorio];
            valoresDisponiveis.RemoveAt(indiceAleatorio);

            ButtonAlternativa.Spawn(respostas.transform, valorAleatorio, perguntaAtual.opcoes[valorAleatorio].resposta, false);
        }

        List<string> indexLetra = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h" };
        for (int i = 0;i < indexLetra.Count; i++)
        {
            if (perguntaAtual.resposta_correta == indexLetra[i])
            {
                botaoCorreto = i; break;
            }
        }
        #endregion
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

    public void ClicarBotaoAlternativa(int alternativa)
    {
        // UPDATE acao = "game_detetive_perguntas" | perguntas_respondidas++ =================================================================================
        GameManager.Instance.perguntasRespondidas++;

        if (alternativa == botaoCorreto)
        {
            animatorCorreto.SetTrigger("click");

            Thread.Sleep(250);
            // UPDATE acao = "game_detetive_perguntas" | perguntas_acertadas++ =================================================================================
            GameManager.Instance.perguntasAcertadas++;
            CaseManager.Instance.playerScore++;
            score.fillAmount = (float)(CaseManager.Instance.playerScore * 0.1);

            if (CaseManager.Instance.playerScore >= 10)
            {
                CaseManager.Instance.playerScore = 0;
                score.fillAmount = 0;
                perguntas.SetActive(false);
                pista.SetActive(true);
                CaseManager.Instance.totalPistas++;

                if (GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas == "")
                {
                    // UPDATE acao = "game_detetive_casos" | locais = CaseManager.Instance.localAtual  =================================================================================
                    GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas += $"{CaseManager.Instance.localAtual}";
                }
                else
                {
                    // UPDATE acao = "game_detetive_casos" | locais += $",{CaseManager.Instance.localAtual}"  =================================================================================
                    GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas += $",{CaseManager.Instance.localAtual}";
                }

                Debug.Log($"Atualizando save Caso: {GameManager.Instance.casos[CaseManager.Instance.indexCasoID].CasoID} | PistasDesbloqueadas: {GameManager.Instance.casos[CaseManager.Instance.indexCasoID].PistasDesbloqueadas}");
            }

            GerarPergunta();
        }
        else RespostaErrada();
    }

    void RespostaErrada()
    {
        animatorErrado.SetTrigger("click");

        Thread.Sleep(250);

        timer = -1;
        respostas.SetActive(false);
        comentario.SetActive(true);
    }

    public void FecharComentario()
    {
        respostas.SetActive(true);
        comentario.SetActive(false);
        GerarPergunta();
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
}