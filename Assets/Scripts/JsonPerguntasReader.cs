using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class JsonPerguntasReader : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        public string pergunta;
        public string resposta_correta;
        public string comentario;
        public List<Options> opcoes = new List<Options>();
    }

    [Serializable]
    public class Options
    {
        public string alternativa;
        public string resposta;
    }

    public List<Question> listaPerguntas = new List<Question>();

    // Primeiro, tornar o Start, ou o método que for fazer a requisição como IEnumetor (assíncrono):
    IEnumerator Start()
    {
        // URL do arquivo JSON remoto
        string url = "https://conradosaud.com.br/outros/game_detetive/perguntas2.json";

        // Cria uma solicitação (request) de busca (GET) usando UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Envia a solicitação e aguarda a resposta
        yield return request.SendWebRequest();

        // Obtém os dados JSON da resposta
        string jsonData = request.downloadHandler.text;

        // Faça o que quiser com os dados JSON
        JObject json = JObject.Parse(jsonData);

        foreach (var pergunta in json)
        {
            var test = pergunta.Value.ToObject<Question>();
            listaPerguntas.Add(test);
        }
    }
}
