using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Networking;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    private JsonCasosReader jsonCasosReader;

    public GameObject painelInicial;
    public GameObject PainelvideoETexto;
    public GameObject text;
    public VideoPlayer videoPlayer;
    public RawImage videoImage;
    public GameObject MultimidiaButtons;
    public GameObject VolumeSlider;
    public GameObject sound;
    public GameObject noSound;
    public Slider slider;
    public GameObject painelDeAviso;
    public TextMeshProUGUI descricaoCaso;

    public int casoNum;
    int casoNumAntigo;
    bool enabledSound = true;
    bool textenabled = true;
    bool temUrl = false;
    private GameManager gameManager;
    

    public List<string> videoURL = new List<string>();


    private void Start()
    {
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        text.SetActive(textenabled);
        videoImage = videoImage.GetComponent<RawImage>();
        
        sound.SetActive(enabledSound);
        noSound.SetActive(!enabledSound);
        
        videoImage.enabled = !textenabled;

        gameManager = GameManager.Instance;
        
        if (gameManager != null)
        {
            videoURL = new List<string>();
            for (int i = 0; i < gameManager.quantidadeCasosJson; i++)
            {
                // Adiciona uma URL vazia para cada caso
                videoURL.Add("");
            }
        }
        else
        {
            Debug.LogError("GameManager não encontrado!");
        }

        if (!GameManager.Instance.gameStarted)
        {
            painelInicial.SetActive(true);
            GameManager.Instance.gameStarted = true;
        } else painelInicial.SetActive(false);

        Debug.Log("Quantidade de Casos no Json: "+GameManager.Instance.quantidadeCasosJson);

        StartCoroutine(PreloadVideos());
    }
    IEnumerator PreloadVideos()
{
    foreach (string url in videoURL)
    {
        if (!string.IsNullOrEmpty(url))
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // Vídeo pré-carregado com sucesso
                    Debug.Log("Vídeo pré-carregado: " + url);
                }
                else
                {
                    // Trate o erro de pré-carregamento do vídeo
                    Debug.LogError("Erro ao pré-carregar vídeo: " + www.error);
                }
            }
        }
    }
}
    void Update()
    {
        if(enabledSound == true)
        {
        videoPlayer.SetDirectAudioVolume(0, slider.value);
        }
        else if (enabledSound == false) 
        {
         videoPlayer.SetDirectAudioVolume(0, 0f);
        }
    if (casoNumAntigo != casoNum)
    {
        videoPlayer.url = null;
        videoPlayer.url = videoURL[casoNum];
    }
    if(string.IsNullOrEmpty(videoURL[casoNum]))
    {
        temUrl = false;
    }
    else
    {
        temUrl = true;
    }
       casoNumAntigo = casoNum;    
       
    }
     public void OnSliderValueChanged()
    {
        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioVolume(0, slider.value);
        }
        else
        {
            Debug.LogWarning("VideoPlayer não atribuído ao MenuInicial.");
        }
    }
    public void Entrar()
    {
        painelInicial.SetActive(false);
    }

    public void Sair()
    {
        Debug.Log("Comando: sair da pagina web atual para a desejada");
        Application.OpenURL("https://www.example.com");
    }

    public void Voltar()
    {
        painelInicial.SetActive(true);
    }

    public void CasoSelecionado(int i)
    {
        casoNum = i;
        if(GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.casoSelecionado = i;
        Debug.Log($"Caso Selecionado: {i}");
        if (i < jsonCasosReader.listaCasos.Count)
        {
            descricaoCaso.text = jsonCasosReader.listaCasos[i].pergunta;
        }
        else
        {
            Debug.LogError("Índice de caso selecionado fora dos limites!");
        }
    }

    public void StartGame()
    {
         PainelvideoETexto.SetActive(true);

        // Inicia a corrotina para reproduzir o vídeo antes de iniciar o jogo
        StartCoroutine(PlayVideoAndLoadScene());
       
    }
    IEnumerator PlayVideoAndLoadScene()
    {
       if (!string.IsNullOrEmpty(videoURL[casoNum]))
    {
        temUrl = true;
        videoPlayer.url = videoURL[casoNum];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        if(videoImage.enabled == true )
        {
        videoPlayer.Play();
        }
        
    }
    else if (string.IsNullOrEmpty(videoURL[casoNum]))
    {
        temUrl = false;
    }
}
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayButton()
    {
        videoPlayer.Play();

    }
    public void PauseButton()
    {
        videoPlayer.Pause();
    }
    public void RestartButton()
    {
         // Parar o vídeo
        videoPlayer.Stop();

        // Voltar para o início
        videoPlayer.frame = 0;

        // Reproduzir o vídeo novamente
        videoPlayer.Play();
    }
    public void EnableSound()
    {
        enabledSound = !enabledSound;
        sound.SetActive(enabledSound);
        noSound.SetActive(!enabledSound);
        VolumeSlider.SetActive(enabledSound);
         
    }
    public void CloseVideoWindow()
    {
        textenabled = true;
        text.SetActive(textenabled);
        videoImage.enabled = false;
        MultimidiaButtons.SetActive(false);
        painelDeAviso.SetActive(false);
        videoPlayer.url = null;
        PainelvideoETexto.SetActive(false);
    }
    public void EnableTextOrVideo()
    {
        textenabled = !textenabled;
        if(temUrl == true)
        {
        text.SetActive(textenabled);
        videoImage.enabled = !textenabled;
        MultimidiaButtons.SetActive(!textenabled);
        painelDeAviso.SetActive(!temUrl);
        }
        else if(temUrl == false)
        {
        text.SetActive(textenabled);    
        videoImage.enabled = temUrl;
        MultimidiaButtons.SetActive(temUrl);
        painelDeAviso.SetActive(!textenabled);
        }
    }
  
}
