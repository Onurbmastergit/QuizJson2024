using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class JsonCasosReader : MonoBehaviour
{
    [Serializable]
    public class Case
    {
        public string nome_do_caso;
        public string pergunta;
        public string url_intro;
        public string resposta_correta;
        public string comentario;
        public List<Options> opcoes = new List<Options>();

        public List<Clues> pistas = new List<Clues>();
    }

    [Serializable]
    public class Options
    {
        public string alternativa;
        public string resposta;
    }

    [Serializable]
    public class Clues
    {
        public string local;
        public string text;
        public List<URL> image = new List<URL>();
    }

    [Serializable]
    public class URL
    {
        public string url;
    }

    public List<Case> listaCasos = new List<Case>();

    IEnumerator Start()
    {
        // Cria uma solicitação (request) de busca (GET) usando UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get("https://conradosaud.com.br/outros/game_detetive/casos.json");
        request.SetRequestHeader("Cache-Control", "no-cache");

        // Envia a solicitação e aguarda a resposta
        yield return request.SendWebRequest();

        // Verifica se houve algum erro na solicitação
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Falha ao baixar o JSON: " + request.error);
            yield break;
        }

        // Obtém os dados JSON da resposta
        string jsonData = request.downloadHandler.text;

        // Faça o que quiser com os dados JSON
        JObject json = JObject.Parse(jsonData);

        // Limpa a lista de casos
        listaCasos.Clear();

        // Itera sobre os casos no JSON e adiciona à lista
        foreach (var caso in json)
        {
            Case casoObjeto = caso.Value.ToObject<Case>();
            listaCasos.Add(casoObjeto);
        }

        // Atualiza a quantidade de casos no GameManager
        GameManager.Instance.quantidadeCasosJson = listaCasos.Count;

        for (int i = 0; i < listaCasos.Count; i++)
        {
            GameManager.Instance.listaIntro.Add(listaCasos[i].url_intro);
        }

        GameManager.Instance.gameStarted = false;
    }
}
