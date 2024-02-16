using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameStarted;

    public int casoSelecionado = 0;

    public int perguntasRespondidas;
    public int perguntasAcertadas;
    public List<string> casosResolvidos = new List<string>();

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
        SceneManager.LoadScene("Menu");
    }
}
