using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject menuDica;
    public GameObject menuCaso;
    public GameObject menuVoltar;
    public GameObject menuPerguntas;

    public int playerScore;

    public bool isMenuOpen;

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

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }


    public void OpenMenuDica()
    {
        if (menuDica.activeSelf)
        {
            isMenuOpen = false;
            menuDica.SetActive(false);
            return;
        }

        isMenuOpen = true;
        menuDica.SetActive(true);
        menuCaso.SetActive(false);
        menuVoltar.SetActive(false);
    }
    public void OpenMenuCaso()
    {
        if (menuCaso.activeSelf)
        {
            isMenuOpen = false;
            menuCaso.SetActive(false);
            return;
        }

        isMenuOpen = true;
        menuDica.SetActive(false);
        menuCaso.SetActive(true);
        menuVoltar.SetActive(false);
    }
    public void OpenMenuVoltar()
    {
        if (menuVoltar.activeSelf)
        {
            isMenuOpen = false;
            menuVoltar.SetActive(false);
            return;
        }

        isMenuOpen = true;
        menuDica.SetActive(false);
        menuCaso.SetActive(false);
        menuVoltar.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
