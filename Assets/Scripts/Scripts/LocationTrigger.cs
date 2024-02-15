using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static JsonCasosReader;

public enum locationNames
{
    financeiro,
    ambulatorio,
    bloco_cirurgico,
    posto_de_enfermagem,
    laboratorio,
    oncologia,
    recepcao,
    pronto_socorro,
    lanchonete,
    uti,
    enfermaria,
    diretoria,
    radiologia,
    farmacia,
    pediatria,
    repouso_medico,
}

public class LocationTrigger : MonoBehaviour
{
    public locationNames ln;

    private JsonCasosReader jsonCasosReader;

    public bool clueUnlocked = false;
    public GameObject alert;

    public TextMeshProUGUI pista;


    void Start()
    {
        // Atribua o componente JsonCasosReader no Editor ou encontre-o dinamicamente
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CaseManager.Instance.localAtual = ln;
        Debug.Log($"Entrando no {ln} {CaseManager.Instance.localAtual}");
        CaseManager.Instance.isMenuOpen = true;
        PistaSelecionada();

        if (clueUnlocked == true)
        {
            CaseManager.Instance.painelPistaRecolhida.SetActive(true);
        }
        else QuestionTrigger();
    }

    void QuestionTrigger()
    {
        CaseManager.Instance.painelPerguntas.SetActive(true);
        CaseManager.Instance.isMenuOpen = true;
        CaseManager.Instance.playerScore = 0;

        clueUnlocked = true;

        if (clueUnlocked)
        {
            alert.SetActive(false);

            Debug.Log($"===== Trigger: PISTA {ln} DESBLOQUEADA =====");
        }

        if (CaseManager.Instance.unlockResolution)
        {
            Debug.Log("===== Trigger: LIBERA O CASO FINAL =====");
        }

        if (CaseManager.Instance.allVisited)
        {
            Debug.Log("===== Trigger: TELA DE CASO FINAL =====");
        }
    }

    void PistaSelecionada()
    {
        int casoSelecionado = GameManager.Instance.casoSelecionado;
        locationNames localAtual = CaseManager.Instance.localAtual;

        var casoAtual = jsonCasosReader.listaCasos[casoSelecionado];

        pista.text = casoAtual.pistas[localAtual.ToString()];
    }
}
