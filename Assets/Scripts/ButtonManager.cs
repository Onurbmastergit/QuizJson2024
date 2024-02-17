using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
  public GameObject buttonPrefab; // Prefab do botão
    public Transform buttonContainer; // Objeto que contém os botões
    private GameManager gameManager; // Referência ao GameManager
    private JsonCasosReader jsonCasosReader; // Referência ao JsonCasosReader

    void Start()
    {
        // Obter as instâncias do GameManager e do JsonCasosReader
        gameManager = GameManager.Instance;
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        // Verificar se o prefab, o container, o GameManager e o JsonCasosReader foram atribuídos
        if (buttonPrefab == null || buttonContainer == null || gameManager == null || jsonCasosReader == null)
        {
            Debug.LogError("Prefab do botão, container, GameManager ou JsonCasosReader não atribuídos!");
            return;
        }

        // Criar os botões
        CreateButtons();
    }

    void CreateButtons()
    {
         // Verificar se há casos no JsonCasosReader
    if (jsonCasosReader.listaCasos.Count == 0)
    {
        Debug.LogError("Nenhum caso encontrado no JsonCasosReader!");
        return;
    }

    // Criar os botões dinamicamente
    for (int i = 0; i < jsonCasosReader.listaCasos.Count; i++)
    {
        // Instanciar o botão a partir do prefab
        GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer);

        // Configurar o texto do botão com o nome do caso do JSON
        buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = jsonCasosReader.listaCasos[i].nome_do_caso;

        // Obter a referência ao componente ButtoFunction
        ButtoFunction buttonFunction = buttonGO.GetComponent<ButtoFunction>();

        // Definir o valor de NumCaso com base na posição do botão
        buttonFunction.NumCaso = i;

        // Adicionar um listener de clique ao botão
        int casoIndex = i; // Criar uma cópia da variável para usar no listener
        buttonGO.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(casoIndex));
    }
    }

    void ButtonClicked(int casoIndex)
    {
        Debug.Log( " Botão " + (casoIndex + 1) + " clicado! Nome do caso: " + jsonCasosReader.listaCasos[casoIndex].nome_do_caso);
    }
}