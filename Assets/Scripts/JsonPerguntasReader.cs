using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonPerguntasReader : MonoBehaviour
{
    public TextAsset textJSON;

    [Serializable]
    public class Question
    {
        public string pergunta;
        public int resposta_correta;
        public string comentario;
        public string[] opcoes;
    }

    public List<Question> listaPerguntas = new List<Question>();

    void Start()
    {
        // Encontra o arquivo
        TextAsset file = Resources.Load<TextAsset>("perguntas");

        // Le o arquivo JSON
        JObject json = JObject.Parse(file.text);

        foreach (var pergunta in json)
        {
            var test = pergunta.Value.ToObject<Question>();
            listaPerguntas.Add(pergunta.Value.ToObject<Question>());
        }
    }
}
