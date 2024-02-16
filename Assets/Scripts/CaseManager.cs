using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class CaseManager : MonoBehaviour
{
    public static CaseManager Instance;

    public GameObject interfaceBackground;

    public GameObject menuDica;
    public GameObject menuCaso;
    public GameObject menuVoltar;

    public GameObject buttonDicas;
    public GameObject buttonCaso;
    public GameObject buttonVoltar;

    public GameObject painelPerguntas;
    public GameObject painelPistaRecolhida;
    public GameObject painelResponderCaso;

    public List<string> pistasDebloqueadas = new List<string>();


    public bool gameStarted;
    public int playerScore;
    public bool isMenuOpen;
    public locationNames localAtual;

    public bool canMove
    {
        get { return !isMenuOpen; }
    }

    public List<string> clueList = new List<string>();

    public LocationTrigger[] locations;
    public bool allVisited
    {
        get
        {
            bool allvisited = false;
            foreach (var location in locations)
            {
                if (location.clueUnlocked)
                {
                    allvisited = true;
                    continue;
                } else
                {
                    allvisited = false;
                    break;
                }
            }
            return allvisited;
        }
    }

    public bool unlockResolution
    {
        get
        {
            int numVisited = 0;
            foreach (var location in locations)
            {
                if (location.clueUnlocked)
                {
                    numVisited++;
                    continue;
                }
            }
            return numVisited >= 12;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    public void OpenMenuDica()
    {
        if (menuDica.activeSelf)
        {
            CaseManager.Instance.isMenuOpen = false;
            interfaceBackground.SetActive(false);

            menuDica.SetActive(false);
            return;
        }

        CaseManager.Instance.isMenuOpen = true;
        interfaceBackground.SetActive(true);

        menuDica.SetActive(true);
        menuCaso.SetActive(false);
        menuVoltar.SetActive(false);
    }
    public void OpenMenuCaso()
    {
        if (menuCaso.activeSelf)
        {
            CaseManager.Instance.isMenuOpen = false;
            interfaceBackground.SetActive(false);

            menuCaso.SetActive(false);
            return;
        }

        CaseManager.Instance.isMenuOpen = true;
        interfaceBackground.SetActive(true);

        menuDica.SetActive(false);
        menuCaso.SetActive(true);
        menuVoltar.SetActive(false);
    }
    public void OpenMenuVoltar()
    {
        if (menuVoltar.activeSelf)
        {
            CaseManager.Instance.isMenuOpen = false;
            interfaceBackground.SetActive(false);

            menuVoltar.SetActive(false);
            return;
        }

        CaseManager.Instance.isMenuOpen = true;
        interfaceBackground.SetActive(true);

        menuDica.SetActive(false);
        menuCaso.SetActive(false);
        menuVoltar.SetActive(true);
    }

    public void ClosePistaRecolhida()
    {
        painelPistaRecolhida.SetActive(false);
        CaseManager.Instance.isMenuOpen = false;
    }

    public void OpenResponderCaso()
    {
        interfaceBackground.SetActive(true);
        painelResponderCaso.SetActive(true);

        menuCaso.SetActive(false);
        buttonCaso.SetActive(false);
        buttonVoltar.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
