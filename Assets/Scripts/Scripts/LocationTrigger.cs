using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum locationNames
{
    financeiro,
    ambulatorio,
    blocoCirurgico,
    postoDeEnfermagem,
    laboratorio,
    oncologia,
    recepcao,
    prontoSocorro,
    lanchonete,
    uti,
    enfermaria,
    diretoria,
    radiologia,
    farmacia,
    pediatria,
    repousoMedico,
}

public class LocationTrigger : MonoBehaviour
{
    public locationNames ln;
    public bool visited = false;
    public GameObject alert;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Entrando no {ln}");

        if (visited == true) return;
        else QuestionTrigger();
    }

    void QuestionTrigger()
    {
        GameManager.Instance.menuPerguntas.SetActive(true);
        GameManager.Instance.isMenuOpen = true;
        GameManager.Instance.playerScore = 0;

        visited = true;
        alert.SetActive(false);
        Debug.Log("===== Trigger: RESPONDA CORRETAMENTE 10 PERGUNTAS =====");

        if (GameManager.Instance.unlockResolution)
        {
            Debug.Log("===== Trigger: LIBERA O CASO FINAL =====");
        }

        if (GameManager.Instance.allVisited)
        {
            Debug.Log("===== Trigger: TELA DE CASO FINAL =====");
        }
    }
}
