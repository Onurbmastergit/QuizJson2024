using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab do botão
    public Transform buttonContainer; // Objeto que contém os botões
    private GameManager gameManager; // Referência ao GameManager
    public JsonCasosReader jsonCasosReader; // Referência ao JsonCasosReader
    public int casosCarregados;


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

        // Verificar se há casos carregados
        if (jsonCasosReader.listaCasos.Count == 0)
        {
            // Se não houver casos carregados, aguarde até que os casos sejam carregados antes de chamar CreateButtons()
            StartCoroutine(WaitForCases());
        }
        else
        {
            // Se os casos já estiverem carregados, chame CreateButtons() imediatamente
            CreateButtons();
        }
    }

    IEnumerator WaitForCases()
    {
        // Aguarde até que os casos sejam carregados
        while (jsonCasosReader.listaCasos.Count == 0)
        { 
            yield return null; // Espere um frame antes de verificar novamente
        }
        casosCarregados = jsonCasosReader.listaCasos.Count; 
        Debug.Log("Casos carregados: " + casosCarregados);
        // Quando os casos forem carregados, chame CreateButtons()
        CreateButtons();
    }

    void CreateButtons()
    {
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
        Debug.Log("Botão " + (casoIndex + 1) + " clicado! Nome do caso: " + jsonCasosReader.listaCasos[casoIndex].nome_do_caso);
    }
}