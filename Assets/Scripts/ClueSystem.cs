using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClueSystem : MonoBehaviour
{
    public TextMeshProUGUI clueText;

    void Update()
    {
        clueText.text = string.Join("\n", GameManager.Instance.clueList);
    }
}
