using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public RectTransform contentPanel;
    public float desiredXPosition = 263.2662f;

    void Start()
    {
        // Define a posição inicial do painel de conteúdo
        SetInitialPosition();
    }

    void SetInitialPosition()
    {
        if (contentPanel != null)
        {
            // Define a posição inicial do painel de conteúdo
            contentPanel.anchoredPosition = new Vector2(desiredXPosition, contentPanel.anchoredPosition.y);
        }
    }
}
