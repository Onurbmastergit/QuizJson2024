using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;

public class SistemaCasos : MonoBehaviour
{
    private JsonCasosReader jsonCasosReader;

    public TextMeshProUGUI casoNome;
    public TextMeshProUGUI casoDescricao;
    public TextMeshProUGUI casoNome1;
    public TextMeshProUGUI casoDescricao1;
    public TextMeshProUGUI casoNome2;
    public TextMeshProUGUI comentarioCaso;

    public GameObject painelPistaRecolhida;

    public TextMeshProUGUI contadorPistas;
    public TextMeshProUGUI instrucaoCaso;
    public GameObject instrucao;
    public GameObject liberarResposta;

    public GameObject painelWin;
    public GameObject painelOver;
    public GameObject painelRespostaCaso;
    public GameObject respostasCaso;
    public GameObject pistaImage;
    public GameObject pistaText;

    int botaoCorreto;
    bool enabledText = true;

    void Start()
    {
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        int casoSelecionado = GameManager.Instance.casoSelecionado;
        locationNames localAtual = CaseManager.Instance.localAtual;

        var casoAtual = jsonCasosReader.listaCasos[casoSelecionado];

        casoNome.text = casoAtual.nome_do_caso;
        casoDescricao.text = casoAtual.pergunta;
        casoNome1.text = casoNome.text;
        casoDescricao1.text = casoDescricao.text;
        casoNome2.text = casoNome.text;
        comentarioCaso.text = casoAtual.comentario;

        // Contador de numero de alternativas atraves do perguntas.json
        int count = casoAtual.opcoes.Count;

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

            ButtonAlternativa.Spawn(respostasCaso.transform, valorAleatorio, casoAtual.opcoes[valorAleatorio].resposta.ToString(), true);
        }

        List<string> indexLetra = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h" };
        for (int i = 0; i < indexLetra.Count; i++)
        {
            if (casoAtual.resposta_correta == indexLetra[i])
            {
                botaoCorreto = i; break;
            }
        }
    }

    // Timer da tela de Vitoria
    float time;
    public Image fill;
    float timerMax;
    bool canSkip;
    public GameObject instrucaoVitoria;
    public GameObject painelVitoria;
    public GameObject painelComentario;
    private void Update()
    {
        if (painelWin.activeSelf)
        {
            time += Time.deltaTime;
            fill.fillAmount = time / timerMax;

            if (time < 0)
            {
                time = 0;
            }

            if (time > timerMax)
            {
                canSkip = true;
            }

            if (canSkip)
            {
                instrucaoVitoria.SetActive(true);
            }

            if (canSkip && Input.anyKey)
            {
                painelVitoria.SetActive(false);
                painelComentario.SetActive(true);
            }
        }
    }

    public void ClicarBotaoAlternativa(int alternativa)
    {
        Thread.Sleep(250);

        if (alternativa == botaoCorreto)
        {
            painelRespostaCaso.SetActive(false);
            painelWin.SetActive(true);
            time = 0;
            timerMax = 30;
        }
        else
        {
            painelRespostaCaso.SetActive(false);
            painelOver.SetActive(true);
        }
    }

    public void PistaSelecionadaSP(int index)
    {
        locationNames[] values = (locationNames[])Enum.GetValues(typeof(locationNames));
        locationNames localAtual = values[index];

        CaseManager.Instance.localAtual = localAtual;

        painelPistaRecolhida.SetActive(true);
    }

    public GameObject[] locais;
    public void DesbloquearPistas()
    {
        for (int i = 0; i < locais.Length; i++)
        {
            string nomeLocal = locais[i].name;

            if (CaseManager.Instance.pistasDebloqueadas.Contains(nomeLocal))
            {
                locais[i].GetComponent<Button>().enabled = true;

                Color currentColor = locais[i].GetComponent<Image>().color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
                locais[i].GetComponent<Image>().color = newColor;
            }
            else
            {
                locais[i].GetComponent<Button>().enabled = false;

                Color currentColor = locais[i].GetComponent<Image>().color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
                locais[i].GetComponent<Image>().color = newColor;
            }
        }

        contadorPistas.text = $"{CaseManager.Instance.totalPistas} / {locais.Length}";
        instrucaoCaso.text = $"Recolha mais {12 - CaseManager.Instance.totalPistas} pistas para tentar solucionar o caso";

        if (CaseManager.Instance.unlockResolution)
        {
            instrucao.SetActive(false);
            liberarResposta.SetActive(true);
        }

        if (CaseManager.Instance.allVisited)
        {
            
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void EnableTextOrImage()
    {
        enabledText = !enabledText;
        pistaText.SetActive(enabledText);
        pistaImage.SetActive(!enabledText);
    }
}
