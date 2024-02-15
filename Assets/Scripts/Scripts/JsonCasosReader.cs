using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonCasosReader : MonoBehaviour
{
    public TextAsset textJSON;

    [Serializable]
    public class Case
    {
        public string nome_do_caso;
        public string pergunta;
        public int resposta_correta;
        public string[] opcoes;

        public Dictionary<string, string> pistas = new Dictionary<string, string>();
    }

    public List<Case> listaCasos = new List<Case>();

    void Start()
    {
        // Encontra o arquivo
        TextAsset file = Resources.Load<TextAsset>("Casos");

        // Le o arquivo JSON
        JObject json = JObject.Parse(file.text);

        foreach (var caso in json)
        {
            var test = caso.Value.ToObject<Case>();
            listaCasos.Add(caso.Value.ToObject<Case>());

            foreach (var item in test.pistas)
            {
                Debug.Log($"{item.Key} - {item.Value}");
            }
        }
    }
}
