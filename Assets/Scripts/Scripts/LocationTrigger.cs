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
    public bool clueUnlocked = false;
    public GameObject alert;

    public string clue;
    public TextMeshProUGUI clueText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Entrando no {ln}");

        if (clueUnlocked == true) return;
        else QuestionTrigger();
    }

    void QuestionTrigger()
    {
        GameManager.Instance.menuPerguntas.SetActive(true);
        GameManager.Instance.isMenuOpen = true;
        GameManager.Instance.playerScore = 0;

        clueUnlocked = true;

        if (clueUnlocked)
        {
            alert.SetActive(false);

            clue = $"Pista {ln} desbloqueada";
            GameManager.Instance.clueList.Add(clue);
            clueText.text = string.Join("\n", GameManager.Instance.clueList);
            Debug.Log($"===== Trigger: PISTA {ln} DESBLOQUEADA =====");
        }

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
