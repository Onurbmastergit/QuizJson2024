using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using static GameManager;

public class API_Manager : MonoBehaviour
{
    public class APIPergunta
    {
        public int id;
        public int id_usuario;
        public int perguntas_respondidas;
        public int perguntas_acertadas;
    }

    public class APICaso
    {
        public int id;
        public int id_usuario;
        public int caso;
        public string locais;
        public int concluido;
    }

    public List<APICaso> apiCasos = new List<APICaso>();

    void Start()
    {
        StartCoroutine(GetRequestPergunta("https://conradosaud.com.br/outros/game_detetive/api/tabela=perguntas&id_usuario=1.json"));
        StartCoroutine(GetRequestCaso("https://conradosaud.com.br/outros/game_detetive/api/tabela=casos&id_usuario=1.json"));
    }

    IEnumerator GetRequestPergunta(string uri)
    {
        string uriWithTimestamp = uri + "?timestamp=" + DateTime.Now.Ticks;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uriWithTimestamp))
        {
            webRequest.SetRequestHeader("Cache-Control", "no-cache");
            webRequest.SetRequestHeader("Pragma", "no-cache");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Something went wrong: {webRequest.error}");
                yield break;
            }

            APIPergunta pergunta = JsonConvert.DeserializeObject<APIPergunta>(webRequest.downloadHandler.text);

            Debug.Log($"Save API - Id_usuario: {pergunta.id_usuario} | Perguntas respondidas: {pergunta.perguntas_respondidas} | Perguntas acertadas: {pergunta.perguntas_acertadas}");

            GameManager.Instance.perguntasRespondidas = pergunta.perguntas_respondidas;
            GameManager.Instance.perguntasAcertadas = pergunta.perguntas_acertadas;
        }
    }

    IEnumerator GetRequestCaso(string uri)
    {
        string uriWithTimestamp = uri + "?timestamp=" + DateTime.Now.Ticks;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uriWithTimestamp))
        {
            webRequest.SetRequestHeader("Cache-Control", "no-cache");
            webRequest.SetRequestHeader("Pragma", "no-cache");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Something went wrong: {webRequest.error}");
                yield break;
            }

            List<APICaso> casos = JsonConvert.DeserializeObject<List<APICaso>>(webRequest.downloadHandler.text);

            foreach (var caso in casos)
            {
                Debug.Log($"Save API - Id_usuario: {caso.id_usuario} | Caso: {caso.caso} | Locais: {caso.locais} | Concluido: {caso.concluido}");

                Caso novoCaso = new Caso(caso.caso, caso.locais, caso.concluido);
                GameManager.Instance.casos.Add(novoCaso);
            }
        }
    }

}
