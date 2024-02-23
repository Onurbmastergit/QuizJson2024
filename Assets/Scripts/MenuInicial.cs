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
    public GameObject painelIntro;
    public GameObject painelVideo;

    public GameObject descricaoCaso;
    public TextMeshProUGUI descricaoCasoText;

    public VideoPlayer videoPlayer;
    public RawImage videoThumbnail;

    public Slider slider;
    public GameObject volumeSlider;
    public GameObject sound;

    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject backButton;
    public GameObject videoButton;

    private void Start()
    {
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();

        if (!GameManager.Instance.gameStarted)
        {
            painelInicial.SetActive(true);
            GameManager.Instance.gameStarted = true;
        } else painelInicial.SetActive(false);

        Debug.Log("Quantidade de Casos no Json: "+GameManager.Instance.quantidadeCasosJson);

        StartCoroutine(PreloadVideos());
    }
    void Update()
    {
        videoPlayer.SetDirectAudioVolume(0, slider.value);
    }

    // =====   Acoes Menu Inicial   =====
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

    // =====   Menu Painel Intro   =====
    IEnumerator PreloadVideos()
    {
        foreach (string url in GameManager.Instance.listaIntro)
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

    public void CasoSelecionado(int i)
    {
        GameManager.Instance.casoSelecionado = i;
        Debug.Log($"Caso Selecionado: {i}");
        descricaoCasoText.text = jsonCasosReader.listaCasos[i].pergunta;

        Debug.Log($"URL Intro: {jsonCasosReader.listaCasos[i].url_intro}");

        if (jsonCasosReader.listaCasos[i].url_intro == "" || jsonCasosReader.listaCasos[i].url_intro == null)
        {
            videoButton.SetActive(false);
        }
        else videoButton.SetActive(true);
    }

    public void OnSliderValueChanged()
    {
        videoPlayer.SetDirectAudioVolume(0, slider.value);
    }

    public void StartGame()
    {
        painelIntro.SetActive(true);

        // Inicia a corrotina para reproduzir o vídeo antes de iniciar o jogo
        StartCoroutine(PlayVideoAndLoadScene());
    }

    IEnumerator PlayVideoAndLoadScene()
    {
        if (GameManager.Instance.listaIntro[GameManager.Instance.casoSelecionado] == null) yield break;

        videoPlayer.url = GameManager.Instance.listaIntro[GameManager.Instance.casoSelecionado];
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        if (videoThumbnail.enabled == true)
        {
            videoPlayer.Play();
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayButton()
    {
        videoPlayer.Play();
        playButton.SetActive(!playButton.activeSelf);
        pauseButton.SetActive(!pauseButton.activeSelf);
    }

    public void PauseButton()
    {
        videoPlayer.Pause();
        playButton.SetActive(!playButton.activeSelf);
        pauseButton.SetActive(!pauseButton.activeSelf);
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
        volumeSlider.SetActive(!volumeSlider.activeSelf);
    }

    public void ButtonVideoWindow()
    {
        painelVideo.SetActive(!painelVideo.activeSelf);
        backButton.SetActive(!backButton.activeSelf);
        descricaoCaso.SetActive(!descricaoCaso.activeSelf);
    }

    public void CloseIntro()
    {
        painelIntro.SetActive(false);
    }
}
