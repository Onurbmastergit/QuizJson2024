using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using static JsonCasosReader;

public enum locationNames
{
    financeiro,
    ambulatorio,
    bloco_cirurgico,
    posto_de_enfermagem,
    laboratorio,
    oncologia,
    recepcao,
    pronto_socorro,
    lanchonete,
    uti,
    enfermaria,
    diretoria,
    radiologia,
    farmacia,
    pediatria,
    repouso_medico,
}

public class LocationTrigger : MonoBehaviour
{
    public locationNames ln;

    JsonCasosReader jsonCasosReader;

    public bool clueUnlocked = false;
    public GameObject verificado;

    public TextMeshProUGUI pista;
    public TextMeshProUGUI local;
    public GameObject imagePrefab; // Prefab para instanciar as imagens
    public Transform container;

    List<string> nomeLocais = new List<string>()
    {
        "Financeiro",
        "Ambulatório",
        "Bloco Cirúrgico",
        "Posto de Enfermagem",
        "Laboratório",
        "Oncologia",
        "Recepção",
        "Pronto Socorro",
        "Lanchonete",
        "UTI",
        "Enfermaria",
        "Diretoria",
        "Radiologia",
        "Farmácia",
        "Pediatria",
        "Repouso Médico"
    };

    void Start()
    {
        // Atribua o componente JsonCasosReader no Editor ou encontre-o dinamicamente
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        for (int i = 0; i < GameManager.Instance.casos.Count; i++)
        {
            if (GameManager.Instance.casos[i].CasoID == GameManager.Instance.casoSelecionado &&
                GameManager.Instance.casos[i].PistasDesbloqueadas != "")
            {
                string pistas = GameManager.Instance.casos[i].PistasDesbloqueadas;

                string[] valores = pistas.Split(",");
                for (int ii = 0; ii < valores.Length; ii++)
                {
                    if (ln.ToString() == valores[ii])
                    {
                        clueUnlocked = true;
                        verificado.SetActive(true);
                        CaseManager.Instance.totalPistas++;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CaseManager.Instance.localAtual = ln;
        Debug.Log($"Entrando no {CaseManager.Instance.localAtual}");

        CaseManager.Instance.isMenuOpen = true;
        PistaSelecionadaLT();

        if (clueUnlocked == true)
        {
            CaseManager.Instance.painelPistaRecolhida.SetActive(true);
        }
        else QuestionTrigger();
    }

    void QuestionTrigger()
    {
        CaseManager.Instance.painelPerguntas.SetActive(true);
        CaseManager.Instance.isMenuOpen = true;

        clueUnlocked = true;
        CaseManager.Instance.pistasDebloqueadas.Add(ln.ToString());

        if (clueUnlocked)
        {
            verificado.SetActive(true);
        }
    }

  public void PistaSelecionadaLT()
    {
        int casoSelecionado = GameManager.Instance.casoSelecionado;
        locationNames localAtual = CaseManager.Instance.localAtual;

        var casoAtual = jsonCasosReader.listaCasos[casoSelecionado];

        pista.text = casoAtual.pistas[(int)localAtual].text;
        local.text = nomeLocais[(int)localAtual];

        // Limpe as imagens anteriores, se houver
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Acesse a lista de URLs de imagens do caso atual e obtenha todas as URLs de imagem correspondentes ao local atual
        List<string> urlsImagens = new List<string>();
        foreach (var url in casoAtual.pistas[(int)localAtual].image)
        {
            urlsImagens.Add(url.url);
        }

        // Carregue e exiba todas as imagens
        foreach (var urlImagem in urlsImagens)
        {
            StartCoroutine(LoadImage(urlImagem));
        }
    }

    IEnumerator LoadImage(string url)
    {
        // Faça o download da imagem usando UnityWebRequest
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        // Verifique se houve algum erro durante o download
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao baixar imagem: " + www.error);
        }
        else
        {
            // Converta a resposta em textura
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // Crie uma nova instância da imagem usando o prefab
            GameObject imageObject = Instantiate(imagePrefab, container);

            // Acesse o componente Image da instância criada
            Image imageComponent = imageObject.GetComponent<Image>();

            // Redimensione a textura para o tamanho desejado
            Texture2D resizedTexture = ResizeTexture(texture, 830, 739);

            // Crie um sprite com a textura redimensionada
            Sprite sprite = Sprite.Create(resizedTexture, new Rect(0, 0, resizedTexture.width, resizedTexture.height), Vector2.zero);

            // Atribua o sprite ao componente Image
            imageComponent.sprite = sprite;
        }
    }

    Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        rt.filterMode = FilterMode.Bilinear;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}

