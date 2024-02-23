using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaCasos : MonoBehaviour
{
    private JsonCasosReader jsonCasosReader;

    public TextMeshProUGUI casoNome;
    public TextMeshProUGUI casoDescricao;
    public TextMeshProUGUI casoNome1;
    public TextMeshProUGUI casoDescricao1;

    public TextMeshProUGUI casoOpcao0;
    public TextMeshProUGUI casoOpcao1;
    public TextMeshProUGUI casoOpcao2;
    public TextMeshProUGUI casoOpcao3;
    public TextMeshProUGUI casoOpcao4;

    public GameObject painelPistaRecolhida;

    public TextMeshProUGUI contadorPistas;
    public TextMeshProUGUI instrucaoCaso;
    public GameObject instrucao;
    public GameObject liberarResposta;

    void Start()
    {
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        int casoSelecionado = GameManager.Instance.casoSelecionado;
        locationNames localAtual = CaseManager.Instance.localAtual;

        var casoAtual = jsonCasosReader.listaCasos[casoSelecionado];

        casoNome.text = casoAtual.nome_do_caso.ToString();
        casoDescricao.text = casoAtual.pergunta.ToString();
        casoNome1.text = casoNome.text;
        casoDescricao1.text = casoDescricao.text;

        casoOpcao0.text = casoAtual.opcoes[0].resposta;
        casoOpcao1.text = casoAtual.opcoes[1].resposta;
        casoOpcao2.text = casoAtual.opcoes[2].resposta;
        casoOpcao3.text = casoAtual.opcoes[3].resposta;
        casoOpcao4.text = casoAtual.opcoes[4].resposta;
    }

    public void PistaSelecionadaSP(int index)
    {
        locationNames[] values = (locationNames[])Enum.GetValues(typeof(locationNames));
        locationNames localAtual = values[index];

        CaseManager.Instance.localAtual = localAtual;

        painelPistaRecolhida.SetActive(true);
    }

    public GameObject[] locais; // Adicione todos os seus GameObjects aqui no Editor Unity

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

    public GameObject painelWin;
    public GameObject painelOver;
    public GameObject painelRespostaCaso;

    public void RepostaCasoCerta()
    {
        painelRespostaCaso.SetActive(false);
        painelWin.SetActive(true);
    }

    public void RepostaCasoErrada()
    {
        painelRespostaCaso.SetActive(false);
        painelOver.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
