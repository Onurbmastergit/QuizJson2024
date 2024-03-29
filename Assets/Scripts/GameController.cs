using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Classe que controla o fluxo do jogo
public class GameController : MonoBehaviour
{
    // Referências aos elementos da interface do usuário
    public TextMeshProUGUI textoPergunta;
    public TextMeshProUGUI textoPontos;
    public TextMeshProUGUI textoTimer;
    public TextMeshProUGUI highScoreText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject painelDePerguntas;
    public GameObject painelFimRodada;

    // Referência ao controlador de dados
    private DataController dataController;

    // Dados da rodada atual
    private RoundData rodadaAtual;

    // Pool de perguntas disponíveis
    private QuestionData[] questionPool;

    // Variáveis para controle da rodada
    private bool rodadaAtiva;
    private float tempoRestante;
    private int questionIndex;
    private int playerScore;

    // Lista de valores de índice já utilizados
    private List<int> usedValues = new List<int>();

    // Lista de objetos de botão de resposta
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    // Método chamado quando o objeto é inicializado
    void Start()
    {
        // Encontra o DataController na cena
        dataController = FindObjectOfType<DataController>();
        // Obtém os dados da rodada atual do controlador de dados
        rodadaAtual = dataController.GetCurrentRoundData();
        // Obtém a pool de perguntas da rodada atual
        questionPool = rodadaAtual.perguntas;
        // Inicializa o tempo restante com o limite de tempo da rodada atual
        tempoRestante = rodadaAtual.limiteDeTempo;

        // Atualiza o temporizador na interface do usuário
        //UpdateTimer();

        // Inicializa a pontuação do jogador e o índice da pergunta
        playerScore = 0;
        questionIndex = 0;

        // Exibe a primeira pergunta
        ShowQuestion();

        // Define a rodada como ativa
        rodadaAtiva = true;
    }

    void Update()
    {
        if (playerScore == 10)
        {
            EndRound();
            
        }
    }

    // Timer Adicionar TimerText se o cliente aceitar o timer
   /* void Update()
    {
        // Verifica se a rodada está ativa
        if (rodadaAtiva)
        {
            // Atualiza o tempo restante
            tempoRestante -= Time.deltaTime;
            UpdateTimer();

            // Verifica se o tempo acabou
            if (tempoRestante <= 0)
            {
                // Finaliza a rodada
                EndRound();    
            }
        }
    }
    */

    // Método para atualizar o temporizador na interface do usuário
    private void UpdateTimer()
    {
        textoTimer.text = "Timer : " + Mathf.Round(tempoRestante).ToString();
    }

    // Método para exibir a próxima pergunta
    private void ShowQuestion()
    {
        // Remove os botões de resposta existentes
        RemoveAnswerButtons();

        // Escolhe uma pergunta aleatória da pool de perguntas que ainda não foi usada
        int random = Random.Range(0 , questionPool.Length);
        while(usedValues.Contains(random))
        {
            random = Random.Range(0 , questionPool.Length);
        }

        // Obtém os dados da pergunta escolhida
        QuestionData questionData = questionPool[random];
        usedValues.Add(random);

        // Exibe o texto da pergunta na interface do usuário
        textoPergunta.text = questionData.textoDaPergunta;

        // Cria botões de resposta para cada resposta disponível na pergunta
        for (int i = 0; i < questionData.respostas.Length; i++)
        {
            // Obtém um objeto de botão de resposta da pool
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            // Define o pai do botão de resposta como o objeto answerButtonParent
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            // Adiciona o botão de resposta à lista de objetos de botão de resposta
            answerButtonGameObjects.Add(answerButtonGameObject);
            // Obtém o componente AnswerButton do botão de resposta
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            // Configura o botão de resposta com os dados da resposta atual
            answerButton.Setuo(questionData.respostas[i]);
        }
    }

    // Método para remover todos os botões de resposta da interface do usuário
    private void RemoveAnswerButtons()
    {
        // Retorna todos os objetos de botão de resposta à pool
        while(answerButtonGameObjects.Count > 0)
        {
           answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
           answerButtonGameObjects.RemoveAt(0);     
        }
    }

    // Método chamado quando um botão de resposta é clicado
    public void AnswerButtonClicked(bool estaCorreto)
    {
        // Verifica se a resposta é correta e atualiza a pontuação do jogador
        if (estaCorreto)
        {
            playerScore += rodadaAtual.pontosPorAcerto;
            textoPontos.text = "Acertos : " + playerScore.ToString();
        }

        // Verifica se há mais perguntas na pool
        if (questionPool.Length > questionIndex + 1)
        {
            // Se sim, exibe a próxima pergunta
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            // Se não, finaliza a rodada
            EndRound();
        }
    }

    // Método para finalizar a rodada
    public void EndRound()
    {
        // Define a rodada como não ativa
        rodadaAtiva = false;

        // Salva a pontuação mais alta do jogador
        dataController.EnviarNovoHighScore(playerScore);
        // Atualiza o texto de pontuação mais alta na interface do usuário
        highScoreText.text = "Perguntas Acertadas : " + dataController.GetHighScore().ToString();

        // Desativa o painel de perguntas e ativa o painel de fim da rodada
        painelDePerguntas.SetActive(false);
        painelFimRodada.SetActive(true);
    }

    // Método para retornar ao menu principal
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}