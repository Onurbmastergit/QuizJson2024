using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ImageLoader : MonoBehaviour
{
    public RawImage backgroundPerguntas;
    public RawImage backgroundPistas;

    public List<RawImage> backgroundLocais = new List<RawImage>();
    public List<string> listaURL = new List<string>();

    void Start()
    {
        StartCoroutine(LoadImagesCoroutine());
    }

    IEnumerator LoadImagesCoroutine()
    {
        for (int i = 0; i < backgroundLocais.Count; i++)
        {
            yield return StartCoroutine(LoadImageCoroutine(listaURL[i], i));
        }
    }

    void Update()
    {
        int index = (int)CaseManager.Instance.localAtual;
        backgroundPerguntas.texture = backgroundLocais[index].texture;
        backgroundPistas.texture = backgroundLocais[index].texture;
    }

    IEnumerator LoadImageCoroutine(string imageUrl, int localIndex)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao carregar a imagem: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Certifique-se de que o índice está dentro dos limites da lista
                if (localIndex < backgroundLocais.Count)
                {
                    backgroundLocais[localIndex].texture = texture;
                }
            }
        }
    }
}