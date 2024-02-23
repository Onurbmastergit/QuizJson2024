using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameStarted;

    // Informa ao sistema qual o caso selecionado pelo usuario
    public int casoSelecionado;

    // Informa ao sistema quantos Casos existem dentro do arquivo casos.json
    public int quantidadeCasosJson;
    public JsonCasosReader jsonCasosReader;
    
    public int perguntasRespondidas;
    public int perguntasAcertadas;

    public List<string> listaIntro = new List<string>();

    public class Caso
    {
        public int CasoID { get; set; }
        public string PistasDesbloqueadas { get; set; }
        public int CasoResolvido { get; set; }

        public Caso(int casoID, string pistasDesbloqueadas, int casoResolvido)
        {
            CasoID = casoID;
            PistasDesbloqueadas = pistasDesbloqueadas;
            CasoResolvido = casoResolvido;
        }
    }

    public List<Caso> casos = new List<Caso>();

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (jsonCasosReader.listaCasos.Count == quantidadeCasosJson)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
