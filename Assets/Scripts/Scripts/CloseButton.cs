using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject gameObject;

    public void closeAction()
    {
        gameObject.SetActive(false);
        GameManager.Instance.isMenuOpen = false;
    }
}
