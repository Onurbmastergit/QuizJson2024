using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using static GameManager;
using UnityEngine.SocialPlatforms;

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

    // Pegar o ID do usuário (use um para testes)
    string id_usuario = "1";

    void Start()
    {
        StartCoroutine(GetRequestPergunta("https://raw.githubusercontent.com/Onurbmastergit/QuizJson2024/Kevin/Assets/Resources/External/tabela%3Dperguntas%26id_usuario%3D1.json"));
        StartCoroutine(GetRequestCaso("https://raw.githubusercontent.com/Onurbmastergit/QuizJson2024/Kevin/Assets/Resources/External/tabela%3Dcasos%26id_usuario%3D1.json"));

        GameManager.Instance.id_usuario = id_usuario;
        //StartCoroutine(GetRequestPergunta($"https://sandbox.edxp.com.br/acesso.php?acao=retorna-perguntas-estudante&estudante={id_usuario}"));
        //StartCoroutine(GetRequestCaso($"https://sandbox.edxp.com.br/acesso.php?acao=retorna-casos-estudante&estudante={id_usuario}"));
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

                /* POST:
                https://sandbox.edxp.com.br/acesso.php

                string acao = "game_detetive_perguntas";
                id_usuario;
                string perguntas_respondidas = "0";
                string perguntas_acertadas = "0";
                */

                //StartCoroutine(GetRequestPergunta($"https://sandbox.edxp.com.br/acesso.php?acao=retorna-perguntas-estudante&estudante={id_usuario}"));

                yield break;
            }

            APIPergunta pergunta = JsonConvert.DeserializeObject<APIPergunta>(webRequest.downloadHandler.text);

            Debug.Log($"Acessando save API - Id_usuario: {pergunta.id_usuario} | Perguntas respondidas: {pergunta.perguntas_respondidas} | Perguntas acertadas: {pergunta.perguntas_acertadas}");
            GameManager.Instance.perguntasRespondidas = pergunta.perguntas_respondidas;
            GameManager.Instance.perguntasAcertadas = pergunta.perguntas_acertadas;
        }

        GameManager.Instance.jsonReady++;
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
                Debug.Log($"Acessando save API - Id_usuario: {caso.id_usuario} | Caso: {caso.caso} | Locais: {caso.locais} | Concluido: {caso.concluido}");

                Caso novoCaso = new Caso(caso.caso, caso.locais, caso.concluido);
                GameManager.Instance.casos.Add(novoCaso);
            }
        }

        GameManager.Instance.jsonReady++;
    }

}
