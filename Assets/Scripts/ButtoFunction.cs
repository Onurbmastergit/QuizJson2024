using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtoFunction : MonoBehaviour
{
    private MenuInicial menuInicial;
    public int NumCaso;
    

    private void Awake()
    {
        // Aqui você precisa encontrar ou instanciar o objeto MenuInicial
        menuInicial = FindObjectOfType<MenuInicial>();
        if (menuInicial == null)
        {
            Debug.LogError("MenuInicial não encontrado!");
        }
    }

    public void ButtonStart()
    {
        // Verifica se menuInicial é nulo antes de chamar os métodos
        if (menuInicial != null)
        {
            menuInicial.StartGame();
            menuInicial.CasoSelecionado(NumCaso);
        }
        else
        {
            Debug.LogError("MenuInicial não foi inicializado!");
        }
    }
}
