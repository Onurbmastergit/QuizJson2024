using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SistemaPistas : MonoBehaviour
{
    public TextMeshProUGUI casoNome;
    public TextMeshProUGUI casoDescricao;
    public TextMeshProUGUI casoNome1;
    public TextMeshProUGUI casoDescricao1;

    public TextMeshProUGUI casoOpcao0;
    public TextMeshProUGUI casoOpcao1;
    public TextMeshProUGUI casoOpcao2;
    public TextMeshProUGUI casoOpcao3;
    public TextMeshProUGUI casoOpcao4;

    private JsonCasosReader jsonCasosReader;

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

        casoOpcao0.text = casoAtual.opcoes[0].ToString();
        casoOpcao1.text = casoAtual.opcoes[1].ToString();
        casoOpcao2.text = casoAtual.opcoes[2].ToString();
        casoOpcao3.text = casoAtual.opcoes[3].ToString();
        casoOpcao4.text = casoAtual.opcoes[4].ToString();
    }
}
