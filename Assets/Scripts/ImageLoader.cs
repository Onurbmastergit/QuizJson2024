using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ImageLoader : MonoBehaviour
{
    public RawImage rawImage;
    public RawImage rawImage2;

    public void LoadRemoteImage(string imageUrl)
    {
        StartCoroutine(LoadImageCoroutine(imageUrl));
    }

    IEnumerator LoadImageCoroutine(string imageUrl)
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
                rawImage.texture = texture;
                rawImage2.texture = texture;
            }
        }
    }
}