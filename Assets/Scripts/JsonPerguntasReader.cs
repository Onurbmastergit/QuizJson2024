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
        public int resposta_correta;
        public string comentario;
        public string[] opcoes;
    }

    public List<Question> listaPerguntas = new List<Question>();

    // Primeiro, tornar o Start, ou o m�todo que for fazer a requisi��o como IEnumetor (ass�ncrono):
    IEnumerator Start()
    {
        // URL do arquivo JSON remoto
        string url = "https://conradosaud.com.br/outros/game_detetive/perguntas.json";

        // Cria uma solicita��o (request) de busca (GET) usando UnityWebRequest
        // Isso se chama consulta HTTP. Nesse projeto ser� usado o GET para buscar o id na API do cliente
        // Mas tamb�m ser� usado POST para criar um usu�rio (que nunca jogou) e o PUT para alterar um que j� existe
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Envia a solicita��o e aguarda a resposta
        yield return request.SendWebRequest();

        // Obt�m os dados JSON da resposta
        string jsonData = request.downloadHandler.text;

        // Fa�a o que quiser com os dados JSON
        JObject json = JObject.Parse(jsonData);

        foreach (var pergunta in json)
        {
            var test = pergunta.Value.ToObject<Question>();
            listaPerguntas.Add(pergunta.Value.ToObject<Question>());
        }
    }
}
